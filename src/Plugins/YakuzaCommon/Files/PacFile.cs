using System.IO;
using System.Text;
using TF.IO;

namespace YakuzaCommon.Files
{
    public class PacFile
    {
        private string _path;

        public PacFile(string path)
        {
            _path = path;
        }

        public void Extract(string outputPath)
        {
            Directory.CreateDirectory(outputPath);
            var logFile = System.IO.Path.Combine(outputPath, "Extract_Data.tf");

            using (var fs = new FileStream(_path, FileMode.Open, FileAccess.Read, FileShare.Read))
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

                    var sizeMsg = input.ReadInt16();
                    var sizeRemainder = input.ReadInt16();

                    input.Seek(offsetMsg, SeekOrigin.Begin);
                    var msg = input.ReadBytes(sizeMsg);

                    var msgFile = Path.Combine(outputPath, $"{i:0000}.msg");
                    File.WriteAllBytes(msgFile, msg);

                    log.Write((int) sizeRemainder);

                    input.Seek(offsetRemainder, SeekOrigin.Begin);
                    var remainder = input.ReadBytes(sizeMsg);

                    log.Write(remainder);
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

                    var msgFile = Path.Combine(inputFolder, $"{i:0000}.msg");
                    var msg = File.ReadAllBytes(msgFile);
                    var sizeRemainder = log.ReadInt32();
                    var remainder = log.ReadBytes(sizeRemainder);

                    output.Write(outputOffset);
                    output.Write(outputOffset + msg.Length);
                    output.Write((short) msg.Length);
                    output.Write((short) sizeRemainder);

                    output.Seek(outputOffset, SeekOrigin.Begin);
                    output.Write(msg);
                    output.Write(remainder);

                    outputOffset = (int) output.Position;
                }
            }
        }
    }
}
