using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using ShenmueDKSharp.Files.Containers;
using TF.IO;



namespace TFGame.Shenmue.Files
{
    public class TacFile
    {
        public static void Extract(string inputPath, string outputFolder)
        {
            var inputFolder = Path.GetDirectoryName(inputPath);
            var fileName = Path.GetFileNameWithoutExtension(inputPath);

            var tadFile = Path.Combine(inputFolder, $"{fileName}.tad");

            if (!File.Exists(tadFile))
            {
                throw new FileNotFoundException($"No se ha encontrado el archivo {tadFile}");
            }

            Directory.CreateDirectory(outputFolder);

            var tad = new TAD(tadFile);
            var tac = new TAC(tad, inputPath);
            tac.Unpack(false, false, outputFolder);
            
            var logFile = Path.Combine(outputFolder, "Extract_Data.tf");

            using (var log = new ExtendedBinaryWriter(new FileStream(logFile, FileMode.Create), System.Text.Encoding.GetEncoding(1252)))
            {
                foreach (var entry in tad.Entries)
                {
                    if (!string.IsNullOrEmpty(entry.FileName))
                    {
                        var path = entry.FileName.Replace("/", "\\");
                        var bytes = File.ReadAllBytes(string.Concat(outputFolder, path));
                        var md5 = MD5Hash(bytes);

                        log.WriteString(entry.FileName);
                        log.Write(entry.FirstHash);
                        log.Write(entry.SecondHash);
                        log.Write(entry.Unknown);
                        log.Write(md5);
                    }
                }
            }
        }

        public static void Repack(string inputFolder, string outputPath, bool useCompression)
        {
            var dir = Path.GetDirectoryName(outputPath);
            Directory.CreateDirectory(dir);

            var tacFileName = Path.GetFileNameWithoutExtension(outputPath);
            var tadFileName = Path.Combine(dir, $"{tacFileName}.tad");
            var tad = new TAD();

            var logFile = Path.Combine(inputFolder, "Extract_Data.tf");

            using (var log = new ExtendedBinaryReader(new FileStream(logFile, FileMode.Open), System.Text.Encoding.GetEncoding(1252)))
            {
                

                while (log.Position < log.Length)
                {
                    var fileName = log.ReadString();
                    var firstHash = log.ReadUInt32();
                    var secondHash = log.ReadUInt32();
                    var unknown = log.ReadUInt32();
                    var originalMD5 = log.ReadBytes(16);

                    var path = fileName.Replace("/", "\\");
                    var completePath = string.Concat(inputFolder, path);
                    var bytes = File.ReadAllBytes(completePath);
                    var md5 = MD5Hash(bytes);

                    if (HasChanged(originalMD5, md5))
                    {
                        var entry = new TADEntry
                        {
                            FileName = fileName, FirstHash = firstHash, SecondHash = secondHash, Unknown = unknown, FilePath = completePath
                        };

                        tad.Entries.Add(entry);
                    }
                }
            }
            
            var tac = new TAC {TAD = tad};
            tac.Pack(outputPath);
            tad.Write(tadFileName);
        }

        private static byte[] MD5Hash(byte[] data)
        {
            using (var md5 = MD5.Create())
            {
                md5.ComputeHash(data, 0, data.Length);
                return md5.Hash;
            }
        }

        private static bool HasChanged(byte[] oldHash, byte[] newHash)
        {
            return oldHash.Length != newHash.Length || oldHash.Where((hashByte, index) => hashByte != newHash[index]).Any();
        }
    }
}
