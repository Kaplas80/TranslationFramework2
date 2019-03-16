using System;
using System.IO;
using System.Linq;
using System.Text;
using TF.IO;

namespace TFGame.TrailsSky.Files
{
    public class DatFile
    {
        private static byte[] DIR_HEADER = {0x4c, 0x42, 0x20, 0x44, 0x49, 0x52, 0x1A, 0x00};
        private static byte[] DAT_HEADER = {0x4c, 0x42, 0x20, 0x44, 0x41, 0x54, 0x1A, 0x00};

        public static void Extract(string inputPath, string outputFolder)
        {
            var inputFolder = Path.GetDirectoryName(inputPath);
            var fileName = Path.GetFileNameWithoutExtension(inputPath);

            var dirFile = Path.Combine(inputFolder, $"{fileName}.dir");

            if (!File.Exists(dirFile))
            {
                throw new FileNotFoundException($"No se ha encontrado el archivo {dirFile}");
            }

            Directory.CreateDirectory(outputFolder);

            using (var dirFs = new FileStream(dirFile, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var dirInput = new ExtendedBinaryReader(dirFs, Encoding.GetEncoding(1252)))
            using (var datFs = new FileStream(inputPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var datInput = new ExtendedBinaryReader(datFs, Encoding.UTF8))
            {
                var dirHeader = dirInput.ReadBytes(8);
                var datHeader = datInput.ReadBytes(8);

                if (!dirHeader.SequenceEqual(DIR_HEADER) || !datHeader.SequenceEqual(DAT_HEADER))
                {
                    throw new InvalidDataException("El fichero no es válido");
                }

                var maxNumFiles = dirInput.ReadUInt64();

                var currentPos = dirInput.Position;
                var endPos = dirInput.Length;

                while (currentPos < endPos)
                {
                    var name = dirInput.ReadString();
                    dirInput.Skip(3);
                    var compressedSize = dirInput.ReadInt32();
                    var maxSize = dirInput.ReadInt32();
                    var unknown2 = dirInput.ReadInt32();
                    var timestamp = dirInput.ReadInt32();

                    var datOffset = dirInput.ReadInt32();

                    if (compressedSize != 0)
                    {
                        var outputFile = Path.Combine(outputFolder, name);

                        datInput.Seek(datOffset, SeekOrigin.Begin);
                        var data = datInput.ReadBytes(compressedSize);
                        try
                        {
                            var data2 = FalcomCompressor.Decompress(data);
                            File.WriteAllBytes(outputFile, data2);
                        }
                        catch (Exception e)
                        {
                            File.WriteAllBytes(outputFile, data);
                        }
                    }

                    currentPos = dirInput.Position;
                }
            }
        }
    }
}
