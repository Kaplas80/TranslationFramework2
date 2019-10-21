using System;
using System.IO;
using System.Reflection;

namespace TFGame.DiscoElysium.Files
{
    using System.Collections.Generic;
    using AssetsTools.NET;
    using AssetsTools.NET.Extra;

    public class UnityFsFile
    {
        public static void Extract(string inputPath, string outputFolder)
        {
            var fileName = Path.GetFileName(inputPath);
            var copyPath = Path.Combine(outputFolder, fileName);

            // Primero tengo que hacer el unpack
            BundleUnpack(inputPath, copyPath);

            // Después hay que extraer el contenido
            BundleExtract(copyPath);
        }

        public static void Repack(string inputFolder, string outputPath, bool useCompression)
        {
            var copyPath = Path.Combine(inputFolder, Path.GetFileName(outputPath));

            BundleRepack(copyPath);

            var dir = Path.GetDirectoryName(outputPath);
            Directory.CreateDirectory(dir);

            File.Copy(copyPath, outputPath);
        }

        private static void BundleUnpack(string packedBundlePath, string unpackedBundlePath)
        {
            using (var inputFs = new FileStream(packedBundlePath, FileMode.Open))
            using (var outputFs = new FileStream(unpackedBundlePath, FileMode.Create))
            {
                var reader = new AssetsFileReader(inputFs);
                var writer = new AssetsFileWriter(outputFs);

                var bundle = new AssetsBundleFile();
                bundle.Unpack(reader, writer);
                bundle.Close();
            }
        }

        private static void BundleExtract(string bundlePath)
        {
            var assetsManager = new AssetsManager(); 
            string cldb = Path.Combine(GetExecutingDirectoryName(), "plugins", "DiscoElysiumDatabase.dat");
            ClassDatabaseFile classFile = assetsManager.LoadClassPackage(cldb);

            string extractPath = Path.Combine(Path.GetDirectoryName(bundlePath), "Unity_Assets_Files");
            Directory.CreateDirectory(extractPath);

            using (var inputFs = new FileStream(bundlePath, FileMode.Open))
            using (var reader = new AssetsFileReader(inputFs))
            {
                
                var bundle = new AssetsBundleFile();
                bundle.Read(reader);

                using (Stream partition = GetPartition(bundle, reader, 0))
                {
                    AssetsFileInstance instance = assetsManager.LoadAssetsFile(partition, Path.GetDirectoryName(bundlePath), false);

                    foreach (AssetFileInfoEx info in instance.table.pAssetFileInfo)
                    {
                        string name = AssetHelper.GetAssetNameFast(instance.file, classFile, info);

                        instance.file.reader.Position = info.absoluteFilePos;
                        byte[] file = instance.file.reader.ReadBytes((int)info.curFileSize);
                        var outputPath = Path.Combine(extractPath, $"{name}_SELFNAME.{info.index}");
                        File.WriteAllBytes(outputPath, file);
                    }
                }
            }
        }

        private static void BundleRepack(string bundlePath)
        {
            var tempPath = Path.GetTempFileName();

            var assetsManager = new AssetsManager(); 
            string cldb = Path.Combine(GetExecutingDirectoryName(), "plugins", "DiscoElysiumDatabase.dat");
            ClassDatabaseFile classFile = assetsManager.LoadClassPackage(cldb);

            string extractPath = Path.Combine(Path.GetDirectoryName(bundlePath), "Unity_Assets_Files");

            using (var inputFs = new FileStream(bundlePath, FileMode.Open))
            using (var reader = new AssetsFileReader(inputFs))
            {
                var bundle = new AssetsBundleFile();
                bundle.Read(reader);

                using (Stream partition = GetPartition(bundle, reader, 0))
                {
                    AssetsFileInstance instance = assetsManager.LoadAssetsFile(partition, Path.GetDirectoryName(bundlePath), false);
                    var replacers = new List<AssetsReplacer>();

                    foreach (AssetFileInfoEx info in instance.table.pAssetFileInfo)
                    {
                        string name = AssetHelper.GetAssetNameFast(instance.file, classFile, info);
                        var filePath = Path.Combine(extractPath, $"{name}_SELFNAME.{info.index}");
                        if (File.Exists(filePath))
                        {
                            AssetsReplacer assetsReplacer = new AssetsReplacerFromMemory(0, info.index, instance.file.typeTree.pTypes_Unity5[info.curFileTypeOrIndex].classId, instance.file.typeTree.pTypes_Unity5[info.curFileTypeOrIndex].scriptIndex, File.ReadAllBytes(filePath));
                            replacers.Add(assetsReplacer);
                        }
                    }

                    using (var ms = new MemoryStream()) 
                    using (var writer = new AssetsFileWriter(ms))
                    {
                        instance.file.Write(writer, 0, replacers.ToArray(), 0);

                        var newData = ms.ToArray();
                        reader.Position = 0;

                        using (var fs = new FileStream(tempPath, FileMode.Create))
                        {
                            var bundleWriter = new AssetsFileWriter(fs);
                            bundle.ReplacePartition(reader, bundleWriter, 0, newData);
                        }
                    }
                }
            }

            File.Copy(tempPath, bundlePath, true);
            File.Delete(tempPath);
        }

        private static Stream GetPartition(AssetsBundleFile bundle, AssetsFileReader reader, int partitionIndex)
        {
            ulong start = (bundle.bundleHeader6.GetFileDataOffset() + bundle.bundleInf6.dirInf[partitionIndex].offset);
            ulong length = bundle.bundleInf6.dirInf[partitionIndex].decompressedSize;

            reader.Position = start;
            byte[] data = reader.ReadBytes((int)length);
            return new MemoryStream(data);
        }

        private static string GetExecutingDirectoryName()
        {
            var location = new Uri(Assembly.GetEntryAssembly().GetName().CodeBase);
            
            return Path.GetDirectoryName(location.LocalPath);
        }
    }
}
