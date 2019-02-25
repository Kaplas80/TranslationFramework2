using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TF.Core.Exceptions;
using TF.IO;
using YakuzaCommon.Files.SimpleSubtitle;

namespace YakuzaCommon.Files.StringTbl
{
    public class File : SimpleSubtitle.File
    {
        public File(string path, string changesFolder, Encoding encoding) : base(path, changesFolder, encoding)
        {
        }

        protected override IList<Subtitle> GetSubtitles()
        {
            if (HasChanges)
            {
                try
                {
                    var loadedSubs = LoadChanges(ChangesFile);
                    return loadedSubs;
                }
                catch (ChangesFileVersionMismatchException e)
                {
                    System.IO.File.Delete(ChangesFile);
                }
            }

            var result = new List<Subtitle>();

            using (var fs = new FileStream(Path, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fs, FileEncoding, Endianness.BigEndian))
            {
                var count1 = input.ReadInt32();
                input.Skip(4);
                
                for (var i = 0; i < count1; i++)
                {
                    var count2 = input.ReadInt32();
                    var offset1 = input.ReadInt32();
                    var returnPos = input.Position;
                    input.Seek(offset1, SeekOrigin.Begin);
                    for (var j = 0; j < count2; j++)
                    {
                        var offset2 = input.ReadInt32();
                        if (offset2 > 0)
                        {
                            result.Add(ReadSubtitle(input, offset2));
                        }
                    }
                    input.Seek(returnPos, SeekOrigin.Begin);
                }
            }

            return result;
        }

        private Subtitle ReadSubtitle(ExtendedBinaryReader input, int offset)
        {
            var result = new Subtitle {Offset = offset};
            var returnPos = input.Position;
            input.Seek(offset, SeekOrigin.Begin);
            result.Text = input.ReadString();
            result.Loaded = result.Text;
            result.Translation = result.Text;
            result.PropertyChanged += SubtitlePropertyChanged;
            input.Seek(returnPos, SeekOrigin.Begin);
            return result;
        }

        public override void Rebuild(string outputFolder)
        {
            var outputPath = System.IO.Path.Combine(outputFolder, RelativePath);
            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(outputPath));

            var subtitles = GetSubtitles();

            using (var fsInput = new FileStream(Path, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fsInput, FileEncoding, Endianness.BigEndian))
            using (var fsOutput = new FileStream(outputPath, FileMode.Create))
            using (var output = new ExtendedBinaryWriter(fsOutput, FileEncoding, Endianness.BigEndian))
            using (var tempMemoryStream = new MemoryStream())
            using (var tempOutput = new ExtendedBinaryWriter(tempMemoryStream, FileEncoding, Endianness.BigEndian))
            {
                var offsets = new List<int>();

                var count1 = input.ReadInt32();
                output.Write(count1);
                output.Write(input.ReadBytes(4));

                var outputOffset = 0;

                for (var i = 0; i < count1; i++)
                {
                    var count2 = input.ReadInt32();
                    output.Write(count2);
                    var offset1 = input.ReadInt32();
                    output.Write(offset1);

                    var returnPos = input.Position;
                    input.Seek(offset1, SeekOrigin.Begin);
                    
                    for (var j = 0; j < count2; j++)
                    {
                        var offset2 = input.ReadInt32();
                        if (offset2 == 0)
                        {
                            offsets.Add(-1);
                        }
                        else 
                        {
                            offsets.Add(outputOffset);
                            outputOffset = WriteString(tempOutput, subtitles, offset2);
                        }
                    }
                    input.Seek(returnPos, SeekOrigin.Begin);
                }

                outputOffset = (int) (output.Position + 4 * offsets.Count);

                foreach (var offset in offsets)
                {
                    if (offset == -1)
                    {
                        output.Write(0);
                    }
                    else
                    {
                        output.Write(outputOffset + offset);
                    }
                }

                var data = tempMemoryStream.ToArray();
                output.Write(data);
            }
        }

        private int WriteString(ExtendedBinaryWriter output, IList<Subtitle> subtitles, int inputOffset)
        {
            var str = subtitles.First(x => x.Offset == inputOffset);
            output.WriteString(str.Translation);
            var result = (int) output.Position;

            return result;
        }
    }
}
