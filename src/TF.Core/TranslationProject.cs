using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TF.Core.Entities;
using TF.Core.Exceptions;
using TF.Core.Helpers;
using TF.Core.POCO;
using TF.IO;

namespace TF.Core
{
    public class TranslationProject
    {
        public IGame Game { get; private set; }
        public string InstallationPath { get; private set; }
        public string WorkPath { get; private set; }

        public string ContainersFolder => Path.Combine(WorkPath, "containers");
        public string ChangesFolder => Path.Combine(WorkPath, "changes");
        public string ExportFolder => Path.Combine(WorkPath, "export");
        public string TempFolder => Path.Combine(WorkPath, "temp");

        public IList<TranslationFileContainer> FileContainers { get; private set; }

        private TranslationProject()
        {
            FileContainers = new List<TranslationFileContainer>();
        }

        public TranslationProject(IGame game, string installFolder, string path) : this()
        {
            Game = game;
            InstallationPath = installFolder;
            WorkPath = path;

            Directory.CreateDirectory(ContainersFolder);
            Directory.CreateDirectory(ChangesFolder);
            Directory.CreateDirectory(ExportFolder);
            Directory.CreateDirectory(TempFolder);
        }

        public void ReadTranslationFiles(BackgroundWorker worker)
        {
            UpdateTranslationFiles(this, worker);
        }

        public void Save()
        {
            var saveFile = Path.Combine(WorkPath, "project.tf");
            using (var fs = new FileStream(saveFile, FileMode.Create))
            using (var output = new ExtendedBinaryWriter(fs, Encoding.UTF8))
            {
                output.WriteString(Game.Id);
                output.Write(Game.Version);
                output.WriteString(InstallationPath);
                output.Write(FileContainers.Count);
                foreach (var container in FileContainers)
                {
                    output.WriteString(container.Id);
                    output.WriteString(container.Path);
                    output.Write((int)container.Type);
                    output.Write(container.Files.Count);
                    foreach (var file in container.Files)
                    {
                        output.WriteString(file.Id);
                        output.WriteString(file.Path);
                        output.WriteString(file.RelativePath);
                        output.WriteString(file.Name);
                        output.WriteString(file.GetType().FullName);
                    }
                }
            }
        }

        public static TranslationProject Load(string path, PluginManager pluginManager, BackgroundWorker worker)
        {
            var types = new Dictionary<string, Type>();

            var searchNewFiles = false;

            using (var fs = new FileStream(path, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fs, Encoding.UTF8))
            {
                var result = new TranslationProject();

                var gameId = input.ReadString();
                var game = pluginManager.GetGame(gameId);
                result.Game = game ?? throw new Exception("No existe un plugin para cargar este fichero.");

                var pluginVersion = input.ReadInt32();
                if (pluginVersion > game.Version)
                {
                    throw new Exception("No coincide la versión del plugin instalado con la versión del que creó esta traducción.");
                }

                if (pluginVersion < game.Version)
                {
                    searchNewFiles = true;
                }

                var installPath = input.ReadString();
                if (!Directory.Exists(installPath))
                {
                    throw new Exception($"No se encuentra la carpeta de instalación: {installPath}");
                }

                result.InstallationPath = installPath;
                result.WorkPath = Path.GetDirectoryName(path);

                var containersCount = input.ReadInt32();
                for (var i = 0; i < containersCount; i++)
                {
                    var containerId = input.ReadString();
                    var containerPath = input.ReadString();
                    var containerType = (ContainerType)input.ReadInt32();
                    var container = new TranslationFileContainer(containerId, containerPath, containerType);

                    var fileCount = input.ReadInt32();
                    for (var j = 0; j < fileCount; j++)
                    {
                        var fileId = input.ReadString();
                        var filePath = input.ReadString();
                        var fileRelativePath = input.ReadString();
                        var fileName = input.ReadString();
                        var typeString = input.ReadString();

                        var type = GetType(typeString, types);

                        TranslationFile file;

                        var constructorInfo =
                            type.GetConstructor(new[] {typeof(string), typeof(string), typeof(string), typeof(Encoding)});

                        if (constructorInfo != null)
                        {
                            file = (TranslationFile)constructorInfo.Invoke(new object[]{game.Name, filePath, result.ChangesFolder, result.Game.FileEncoding});
                        }
                        else
                        {
                            constructorInfo = type.GetConstructor(new[] {typeof(string), typeof(string), typeof(string)});
                            if (constructorInfo != null)
                            {
                                file = (TranslationFile)constructorInfo.Invoke(new object[]{game.Name, filePath, result.ChangesFolder});
                            }
                            else
                            {
                                file = new TranslationFile(game.Name, filePath, result.ChangesFolder);
                            }
                        }

                        file.Id = fileId;
                        file.Name = fileName;
                        file.RelativePath = fileRelativePath;
                        container.AddFile(file);
                    }

                    result.FileContainers.Add(container);
                }

                if (searchNewFiles)
                {
                    UpdateTranslationFiles(result, worker);
                }

                return result;
            }
        }

        private static Type GetType(string typeName, Dictionary<string, Type> types)
        {
            if (types.ContainsKey(typeName))
            {
                return types[typeName];
            }

            var type = Type.GetType(typeName);

            if (type != null)
            {
                types.Add(typeName, type);
                return type;
            }

            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = a.GetType(typeName);
                if (type != null)
                {
                    types.Add(typeName, type);
                    return type;
                }
            }

            return null;
        }

        public void Export(IList<TranslationFileContainer> containers, ExportOptions options, BackgroundWorker worker)
        {
            PathHelper.DeleteDirectory(TempFolder);

            foreach (var container in containers)
            {
                if (worker.CancellationPending)
                {
                    worker.ReportProgress(0, "CANCELADO");
                    throw new UserCancelException();
                }

                worker.ReportProgress(0, $"Procesando {container.Path}...");

                if (container.Type == ContainerType.Folder)
                {
                    var outputFolder = Path.GetFullPath(Path.Combine(ExportFolder, container.Path));

                    foreach (var translationFile in container.Files)
                    {
                        if (translationFile.HasChanges || options.ForceRebuild)
                        {
                            translationFile.Rebuild(outputFolder);
                        }
                        else
                        {
                            var outputFile = Path.Combine(outputFolder, translationFile.RelativePath);
                            var dir = Path.GetDirectoryName(outputFile);
                            Directory.CreateDirectory(dir);

                            File.Copy(translationFile.Path, outputFile, true);
                        }
                    }
                }
                else
                {
                    var outputFile = Path.GetFullPath(Path.Combine(ExportFolder, container.Path));
                    if (File.Exists(outputFile))
                    {
                        File.Delete(outputFile);
                    }

                    worker.ReportProgress(0, "Preparando para empaquetar...");
                    // 1. Copiar todos los ficheros del contenedor a una carpeta temporal
                    var source = Path.Combine(ContainersFolder, container.Id);
                    var dest = Path.Combine(TempFolder, container.Id);
                    Directory.CreateDirectory(dest);

                    PathHelper.CloneDirectory(source, dest);

                    // 2. Crear los ficheros traducidos en esa carpeta temporal
                    worker.ReportProgress(0, "Generando ficheros traducidos...");
                    foreach (var translationFile in container.Files)
                    {
                        if (translationFile.HasChanges || options.ForceRebuild)
                        {
                            translationFile.Rebuild(dest);
                        }
                    }

                    // 3. Empaquetar
                    worker.ReportProgress(0, "Empaquetando fichero...");
                    Game.RepackFile(dest, outputFile, options.UseCompression);

                    // 4. Eliminar la carpeta temporal
                    if (!options.SaveTempFiles)
                    {
                        try
                        {
                            Directory.Delete(dest, true);
                        }
                        catch (IOException)
                        {
                            Thread.Sleep(0);
                            Directory.Delete(dest, true);
                        }
                    }
                }
            }
        }

        public IList<Tuple<TranslationFileContainer, TranslationFile>> SearchInFiles(string searchString, BackgroundWorker worker)
        {
            var result = new ConcurrentBag<Tuple<TranslationFileContainer, TranslationFile>>();

            foreach (var container in FileContainers)
            {
                if (worker.CancellationPending)
                {
                    worker.ReportProgress(0, "CANCELADO");
                    throw new UserCancelException();
                }

                worker.ReportProgress(0, $"Procesando {container.Path}...");

                //foreach (var file in container.Files)
                Parallel.ForEach(container.Files, file =>
                {
                    var found = file.Search(searchString);

                    if (found)
                    {
                        result.Add(new Tuple<TranslationFileContainer, TranslationFile>(container, file));
                    }
                });

            }

            return result.ToList();
        }

        private static void UpdateTranslationFiles(TranslationProject project, BackgroundWorker worker)
        {
            var containers = project.Game.GetContainers(project.InstallationPath);
            foreach (var container in containers)
            {
                if (worker.CancellationPending)
                {
                    worker.ReportProgress(0, "CANCELADO");
                    throw new UserCancelException();
                }

                var translationContainer =
                    project.FileContainers.FirstOrDefault(x => x.Path == container.Path && x.Type == container.Type);

                var addNewContainer = false;
                var addedFiles = 0;

                if (translationContainer == null)
                {
                    translationContainer = new TranslationFileContainer(container.Path, container.Type);
                    addNewContainer = true;
                }

                var extractionContainerPath = Path.Combine(project.ContainersFolder, translationContainer.Id);
                Directory.CreateDirectory(extractionContainerPath);

                var containerPath = Path.GetFullPath(Path.Combine(project.InstallationPath, container.Path));

                worker.ReportProgress(0, $"Procesando {container.Path}...");
                if (container.Type == ContainerType.CompressedFile)
                {
                    if (File.Exists(containerPath))
                    {
                        if (addNewContainer)
                        {
                            project.Game.PreprocessContainer(translationContainer, containerPath, extractionContainerPath);
                            project.Game.ExtractFile(containerPath, extractionContainerPath);
                        }

                        foreach (var fileSearch in container.FileSearches)
                        {
                            worker.ReportProgress(0, $"Buscando {fileSearch.RelativePath}\\{fileSearch.SearchPattern}...");
                            var foundFiles = fileSearch.GetFiles(extractionContainerPath);

                            //foreach (var f in foundFiles)
                            Parallel.ForEach(foundFiles, f =>
                            {
                                var relativePath =
                                    PathHelper.GetRelativePath(extractionContainerPath, Path.GetFullPath(f));
                                var type = fileSearch.FileType;

                                var translationFile =
                                    translationContainer.Files.FirstOrDefault(x => x.RelativePath == relativePath);

                                if (translationFile == null)
                                {
                                    var constructorInfo =
                                        type.GetConstructor(new[] {typeof(string), typeof(string), typeof(string), typeof(Encoding)});

                                    if (constructorInfo != null)
                                    {
                                        translationFile = (TranslationFile)constructorInfo.Invoke(new object[]{project.Game.Name, f, project.ChangesFolder, project.Game.FileEncoding});
                                    }
                                    else
                                    {
                                        constructorInfo = type.GetConstructor(new[] {typeof(string), typeof(string), typeof(string)});
                                        if (constructorInfo != null)
                                        {
                                            translationFile = (TranslationFile)constructorInfo.Invoke(new object[]{project.Game.Name, f, project.ChangesFolder});
                                        }
                                        else
                                        {
                                            translationFile = new TranslationFile(project.Game.Name, f, project.ChangesFolder);
                                        }
                                    }

                                    translationFile.RelativePath = relativePath;

                                    if (translationFile.Type == FileType.TextFile)
                                    {
                                        if (translationFile.SubtitleCount > 0)
                                        {
                                            translationContainer.AddFile(translationFile);
                                            addedFiles++;
                                        }
                                    }
                                    else
                                    {
                                        translationContainer.AddFile(translationFile);
                                        addedFiles++;
                                    }
                                }
                            });
                        }

                        project.Game.PostprocessContainer(translationContainer, containerPath, extractionContainerPath);

                        worker.ReportProgress(0, $"{addedFiles} ficheros encontrados y añadidos");
                    }
                    else
                    {
                        worker.ReportProgress(0, $"ERROR: No existe el fichero comprimido {containerPath}");
                        continue;
                    }
                }
                else
                {
                    project.Game.PreprocessContainer(translationContainer, containerPath, extractionContainerPath);
                    foreach (var fileSearch in container.FileSearches)
                    {
                        worker.ReportProgress(0, $"Buscando {fileSearch.RelativePath}\\{fileSearch.SearchPattern}...");
                        var foundFiles = fileSearch.GetFiles(containerPath);

                        //foreach (var f in foundFiles)
                        Parallel.ForEach(foundFiles, f =>
                        {
                            var relativePath = PathHelper.GetRelativePath(containerPath, Path.GetFullPath(f));

                            var destinationFileName =
                                Path.GetFullPath(Path.Combine(extractionContainerPath, relativePath));
                            var destPath = Path.GetDirectoryName(destinationFileName);
                            Directory.CreateDirectory(destPath);

                            if (!File.Exists(destinationFileName))
                            {
                                File.Copy(f, destinationFileName);
                            }

                            var type = fileSearch.FileType;

                            var translationFile =
                                translationContainer.Files.FirstOrDefault(x => x.RelativePath == relativePath);

                            if (translationFile == null)
                            {
                                var constructorInfo =
                                    type.GetConstructor(new[] {typeof(string), typeof(string), typeof(string), typeof(Encoding)});

                                if (constructorInfo != null)
                                {
                                    translationFile = (TranslationFile)constructorInfo.Invoke(new object[]{project.Game.Name, destinationFileName, project.ChangesFolder, project.Game.FileEncoding});
                                }
                                else
                                {
                                    constructorInfo = type.GetConstructor(new[] {typeof(string), typeof(string), typeof(string)});
                                    if (constructorInfo != null)
                                    {
                                        translationFile = (TranslationFile)constructorInfo.Invoke(new object[]{project.Game.Name, destinationFileName, project.ChangesFolder});
                                    }
                                    else
                                    {
                                        translationFile = new TranslationFile(project.Game.Name, destinationFileName, project.ChangesFolder);
                                    }
                                }

                                translationFile.RelativePath = relativePath;

                                if (translationFile.Type == FileType.TextFile)
                                {
                                    if (translationFile.SubtitleCount > 0)
                                    {
                                        translationContainer.AddFile(translationFile);
                                        addedFiles++;
                                    }
                                }
                                else
                                {
                                    translationContainer.AddFile(translationFile);
                                    addedFiles++;
                                }
                            }
                        });

                        project.Game.PostprocessContainer(translationContainer, containerPath, extractionContainerPath);
                        worker.ReportProgress(0, $"{addedFiles} ficheros encontrados y añadidos");
                    }
                }

                if (addNewContainer && translationContainer.Files.Count > 0)
                {
                    project.FileContainers.Add(translationContainer);
                }
            }
        }

        public void ExportPo(string path, BackgroundWorker worker)
        {
            foreach (var container in FileContainers)
            {
                if (worker.CancellationPending)
                {
                    worker.ReportProgress(0, "CANCELADO");
                    throw new UserCancelException();
                }

                worker.ReportProgress(0, $"Procesando {container.Path}...");

#if DEBUG
                foreach (var file in container.Files)
                {
                    Debug.WriteLine($"Exporting {Path.Combine(path, container.Path, file.RelativePath)}");
#else
                Parallel.ForEach(container.Files, file =>
                {
#endif
                    var filePath = Path.Combine(path, container.Path, file.RelativePath);
                    //var filePath = Path.Combine(path, file.RelativePath);
                    var fileName = Path.GetFileNameWithoutExtension(filePath);
                    var outputPath = Path.Combine(Path.GetDirectoryName(filePath), string.Concat(fileName, ".po"));
                    file.ExportPo(outputPath);
#if DEBUG
                }
#else
                });
#endif
            }
        }

        public void ImportPo(string path, BackgroundWorker worker)
        {
            foreach (var container in FileContainers)
            {
                if (worker.CancellationPending)
                {
                    worker.ReportProgress(0, "CANCELADO");
                    throw new UserCancelException();
                }

                worker.ReportProgress(0, $"Procesando {container.Path}...");

#if DEBUG
                foreach (var file in container.Files)
#else
                Parallel.ForEach(container.Files, file =>
#endif
                {
                    var filePath = Path.Combine(path, container.Path, file.RelativePath);
                    var fileName = Path.GetFileNameWithoutExtension(filePath);
                    var inputPath = Path.Combine(Path.GetDirectoryName(filePath), string.Concat(fileName, ".po"));

                    if (!File.Exists(inputPath))
                    {
                        filePath = Path.Combine(path, file.RelativePath);
                        inputPath = Path.Combine(Path.GetDirectoryName(filePath), string.Concat(fileName, ".po"));
                    }

                    if (File.Exists(inputPath))
                    {
                        file.ImportPo(inputPath);
                    }
#if DEBUG
                }
#else
                });
#endif
            }
        }
    }
}
