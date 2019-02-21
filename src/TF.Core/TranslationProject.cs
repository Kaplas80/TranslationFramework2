using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using TF.Core.Entities;
using TF.Core.Helpers;
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
            var containers = Game.GetContainers(InstallationPath);
            foreach (var container in containers)
            {
                if (worker.CancellationPending)
                {
                    worker.ReportProgress(0, "CANCELADO");
                    throw new Exception("Cancelado por el usuario");
                }

                var translationContainer = new TranslationFileContainer(container.Path, container.Type);

                var extractionContainerPath = Path.Combine(ContainersFolder, translationContainer.Id);
                Directory.CreateDirectory(extractionContainerPath);

                var containerPath = Path.Combine(InstallationPath, container.Path);

                worker.ReportProgress(0, $"Procesando {container.Path}...");
                if (container.Type == ContainerType.CompressedFile)
                {
                    if (File.Exists(containerPath))
                    {
                        Game.ExtractFile(containerPath, extractionContainerPath);
                        foreach (var fileSearch in container.FileSearches)
                        {
                            worker.ReportProgress(0, $"Buscando {container.Path}\\{fileSearch.RelativePath}\\{fileSearch.SearchPattern}...");
                            var foundFiles = fileSearch.GetFiles(extractionContainerPath);

                            foreach (var f in foundFiles)
                            {
                                var relativePath = PathHelper.GetRelativePath(extractionContainerPath, Path.GetFullPath(f));
                                var translationFile = Game.GetFile(f, this.ChangesFolder) ?? new TranslationFile(f, this.ChangesFolder);
                                translationFile.RelativePath = relativePath;

                                translationContainer.AddFile(translationFile);
                            }

                            worker.ReportProgress(0, $"{translationContainer.Files.Count} ficheros encontrados y añadidos");
                        }
                    }
                    else
                    {
                        worker.ReportProgress(0, $"ERROR: No existe el fichero comprimido {containerPath}");
                        continue;
                    }
                }
                else
                {
                    foreach (var fileSearch in container.FileSearches)
                    {
                        worker.ReportProgress(0, $"Buscando {container.Path}\\{fileSearch.RelativePath}\\{fileSearch.SearchPattern}...");
                        var foundFiles = fileSearch.GetFiles(containerPath);

                        foreach (var f in foundFiles)
                        {
                            var relativePath = PathHelper.GetRelativePath(containerPath, Path.GetFullPath(f));

                            var destinationFileName = Path.Combine(extractionContainerPath, relativePath);
                            var destPath = Path.GetDirectoryName(destinationFileName);
                            Directory.CreateDirectory(destPath);
                            File.Copy(f, Path.Combine(extractionContainerPath, relativePath));

                            var translationFile = Game.GetFile(destinationFileName, this.ChangesFolder) ?? new TranslationFile(destinationFileName, this.ChangesFolder);
                            translationFile.RelativePath = relativePath;

                            translationContainer.AddFile(translationFile);
                        }

                        worker.ReportProgress(0, $"{translationContainer.Files.Count} ficheros encontrados y añadidos");
                    }
                }

                FileContainers.Add(translationContainer);
            }
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
                    }
                }
            }
        }

        public static TranslationProject Load(string path, PluginManager pluginManager)
        {
            using (var fs = new FileStream(path, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fs, Encoding.UTF8))
            {
                var result = new TranslationProject();

                var gameId = input.ReadString();
                var game = pluginManager.GetGame(gameId);
                result.Game = game ?? throw new Exception("No existe un plugin para cargar este fichero.");

                var pluginVersion = input.ReadInt32();
                if (pluginVersion != game.Version)
                {
                    throw new Exception("No coincide la versión del plugin instalado con la versión del que creó esta traducción.");
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
                        
                        var file = game.GetFile(filePath, result.ChangesFolder);
                        file.Id = fileId;
                        file.Name = fileName;
                        file.RelativePath = fileRelativePath;
                        container.AddFile(file);
                    }

                    result.FileContainers.Add(container);
                }

                return result;
            }
        }

        public void Export(IList<TranslationFileContainer> containers, bool useCompression, BackgroundWorker worker)
        {
            Directory.Delete(TempFolder, true);
            Directory.CreateDirectory(TempFolder);

            foreach (var container in containers)
            {
                if (worker.CancellationPending)
                {
                    worker.ReportProgress(0, "CANCELADO");
                    throw new Exception("Cancelado por el usuario");
                }

                worker.ReportProgress(0, $"Procesando {container.Path}...");

                if (container.Type == ContainerType.Folder)
                {
                    var outputFolder = Path.Combine(ExportFolder, container.Path);
                    if (Directory.Exists(outputFolder))
                    {
                        Directory.Delete(outputFolder, true);
                    }

                    foreach (var translationFile in container.Files)
                    {
                        translationFile.Rebuild(outputFolder);
                    }
                }
                else
                {
                    var outputFile = Path.Combine(ExportFolder, container.Path);
                    if (File.Exists(outputFile))
                    {
                        File.Delete(outputFile);
                    }

                    worker.ReportProgress(0, "Preparando para empaquetar...");
                    // 1. Copiar todos los ficheros del contenedor a una carpeta temporal
                    var source = Path.Combine(ContainersFolder, container.Id);
                    var dest = Path.Combine(TempFolder, container.Id);
                    PathHelper.CloneDirectory(source, dest);

                    // 2. Crear los ficheros traducidos en esa carpeta temporal
                    foreach (var translationFile in container.Files)
                    {
                        translationFile.Rebuild(dest);
                    }

                    // 3. Empaquetar
                    worker.ReportProgress(0, "Empaquetando fichero...");
                    Game.RepackFile(dest, outputFile, useCompression);

                    // 4. Eliminar la carpeta temporal
#if !DEBUG
                    Directory.Delete(dest, true);
#endif
                }
            }
        }

        public IList<Tuple<TranslationFileContainer, TranslationFile>> SearchInFiles(string searchString, BackgroundWorker worker)
        {
            var result = new List<Tuple<TranslationFileContainer, TranslationFile>>();

            foreach (var container in FileContainers)
            {
                if (worker.CancellationPending)
                {
                    worker.ReportProgress(0, "CANCELADO");
                    throw new Exception("Cancelado por el usuario");
                }

                worker.ReportProgress(0, $"Procesando {container.Path}...");

                foreach (var file in container.Files)
                {
                    var found = file.Search(searchString);

                    if (found)
                    {
                        result.Add(new Tuple<TranslationFileContainer, TranslationFile>(container, file));
                    }
                }

            }

            return result;
        }
    }
}
