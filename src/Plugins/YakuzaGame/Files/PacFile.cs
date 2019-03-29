using System.IO;
using System.Text;
using TF.IO;

namespace YakuzaGame.Files
{
    public class PacFile
    {
        private PacFile()
        {
        }

        public static void Extract(string inputPath, string outputFolder)
        {
            Directory.CreateDirectory(outputFolder);
            var logFile = System.IO.Path.Combine(outputFolder, "Extract_Data.tf");

            using (var fs = new FileStream(inputPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var input = new ExtendedBinaryReader(fs, Encoding.UTF8, Endianness.BigEndian))
            using (var log = new ExtendedBinaryWriter(new FileStream(logFile, FileMode.Create), Encoding.UTF8, Endianness.BigEndian))
            {
                var numFiles = input.ReadInt16();
                log.Write((int)numFiles);

                log.Write(input.ReadBytes(6));

                for (var i = 0; i < numFiles; i++)
                {
                    input.Seek(8 + i * 16, SeekOrigin.Begin);

                    var unknown = input.ReadInt32();
                    log.Write(unknown);

                    var offsetMsg = input.ReadInt32();
                    var offsetRemainder = input.ReadInt32();

                    log.Write(offsetMsg == 0 ? 0 : 1);
                    log.Write(offsetRemainder == 0 ? 0 : 1);

                    var sizeMsg = input.ReadInt16();
                    var sizeRemainder = input.ReadInt16();

                    if (offsetMsg > 0)
                    {
                        input.Seek(offsetMsg, SeekOrigin.Begin);
                        var msg = input.ReadBytes(sizeMsg);

                        var msgFile = Path.Combine(outputFolder, $"{i:0000}.msg");
                        File.WriteAllBytes(msgFile, msg);
                    }

                    if (offsetRemainder > 0)
                    {
                        log.Write((int) sizeRemainder);

                        input.Seek(offsetRemainder, SeekOrigin.Begin);
                        var remainder = input.ReadBytes(sizeRemainder);

                        log.Write(remainder);
                    }
                }
            }
        }

        public static void Repack(string inputFolder, string outputPath, bool useCompression)
        {
            var logFile = Path.Combine(inputFolder, "Extract_Data.tf");

            var dir = Path.GetDirectoryName(outputPath);
            Directory.CreateDirectory(dir);

            using (var output = new ExtendedBinaryWriter(new FileStream(outputPath, FileMode.Create), Encoding.UTF8, Endianness.BigEndian))
            using (var log = new ExtendedBinaryReader(new FileStream(logFile, FileMode.Open), Encoding.UTF8, Endianness.BigEndian))
            {
                var numFiles = log.ReadInt32();

                output.Write((short)numFiles);
                output.Write(log.ReadBytes(6));

                var outputOffset = 8 + 16 * numFiles;

                for (var i = 0; i < numFiles; i++)
                {
                    output.Seek(8 + i * 16, SeekOrigin.Begin);

                    var unknown = log.ReadInt32();
                    output.Write(unknown);

                    var hasMsg = log.ReadInt32() != 0;
                    var hasRemainder = log.ReadInt32() != 0;

                    if (hasMsg)
                    {
                        var msgFile = Path.Combine(inputFolder, $"{i:0000}.msg");
                        var msg = File.ReadAllBytes(msgFile);

                        output.Write(outputOffset);

                        var returnPos = output.Position;
                        output.Seek(outputOffset, SeekOrigin.Begin);
                        output.Write(msg);
                        outputOffset = (int)output.Position;
                        output.Seek(returnPos + 4, SeekOrigin.Begin);
                        output.Write((short) msg.Length);
                        output.Seek(-6, SeekOrigin.Current);
                    }
                    else
                    {
                        output.Write(0);
                        output.Skip(4);
                        output.Write((short)0);
                        output.Seek(-6, SeekOrigin.Current);
                    }

                    if (hasRemainder)
                    {
                        var sizeRemainder = log.ReadInt32();
                        var remainder = log.ReadBytes(sizeRemainder);

                        output.Write(outputOffset);

                        var returnPos = output.Position;
                        output.Seek(outputOffset, SeekOrigin.Begin);
                        output.Write(remainder);
                        outputOffset = (int)output.Position;
                        output.Seek(returnPos + 2, SeekOrigin.Begin);
                        output.Write((short)sizeRemainder);
                    }
                    else
                    {
                        output.Write(0);
                        output.Skip(2);
                        output.Write((short)0);
                    }
                }
            }
        }
    }
}
