using System;
using System.IO;
using CriCpkMaker;
using TF.IO;

namespace CRIWareGame.Files
{
    public class CpkFile
    {
        public static void Extract(string inputPath, string outputFolder)
        {
            Directory.CreateDirectory(outputFolder);

            var logFile = Path.Combine(outputFolder, "Extract_Data.tf");

            var cpkMaker = InitializeCpkMaker();
            cpkMaker.AnalyzeCpkFile(inputPath);
            cpkMaker.StartToExtract(outputFolder);

            using (var log = new ExtendedBinaryWriter(new FileStream(logFile, FileMode.Create)))
            {
                log.Write(cpkMaker.DataAlign);
                log.Write(cpkMaker.Mask ? 1 : 0);
                log.Write(cpkMaker.FileData.CompressedFiles > 0 ? 1 : 0);
                log.Write((int)cpkMaker.CompressCodec);
                log.Write(cpkMaker.EnableMself ? 1 : 0);
                log.Write((int)cpkMaker.CpkFileMode);
            }

            var status = cpkMaker.Execute();
            while (status != Status.Complete)
            {
                status = cpkMaker.Execute();
            }
        }

        public static void Repack(string inputFolder, string outputPath, bool useCompression)
        {
            var dir = Path.GetDirectoryName(outputPath);
            Directory.CreateDirectory(dir);

            var logFile = Path.Combine(inputFolder, "Extract_Data.tf");

            var cpkMaker = InitializeCpkMaker();
            cpkMaker.ClearFile();
            cpkMaker.BaseDirectory = inputFolder;
            cpkMaker.ToolVersion = "TF2";

            var files = Directory.EnumerateFiles(inputFolder, "*.*", SearchOption.AllDirectories);
            var i = 0u;
            foreach (var file in files)
            {
                var filename = file.Substring(inputFolder.Length);
                if (!file.EndsWith("Extract_Data.tf"))
                {
                    cpkMaker.AddFile(file, filename, i);
                    i++;
                }
            }

            using (var log = new ExtendedBinaryReader(new FileStream(logFile, FileMode.Open)))
            {
                cpkMaker.DataAlign = log.ReadUInt32();
                cpkMaker.Mask = log.ReadInt32() == 1;
                cpkMaker.ForceCompress = (log.ReadInt32() == 1) && useCompression;
                cpkMaker.CompressCodec = (EnumCompressCodec) log.ReadInt32();
                cpkMaker.EnableMself = log.ReadInt32() == 1;
                cpkMaker.CpkFileMode= (CpkMaker.EnumCpkFileMode)log.ReadInt32();
            }

            cpkMaker.StartToBuild(outputPath);

            var status = cpkMaker.Execute();
            while (status != Status.Complete)
            {
                status = cpkMaker.Execute();
            }
        }

        private static CpkMaker InitializeCpkMaker()
        {
            var externalBuffer = new CExternalBuffer();
            if (!externalBuffer.SetBuffers(134217728, 268435456, 134217728))
            {
                externalBuffer.Dispose();

                throw new Exception("Can't allocate memory enough for working.");
            }
            var cpkMaker = new CpkMaker(true);
            return cpkMaker;
        }
    }
}
