using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TF.Core.Files;
using TF.Core.TranslationEntities;
using TF.IO;

namespace YakuzaGame.Files.StringTbl
{
    public class File : BinaryTextFileWithOffsetTable
    {
        public File(string path, string changesFolder, System.Text.Encoding encoding) : base(path, changesFolder, encoding)
        {
        }

        protected override IList<Subtitle> GetSubtitles()
        {
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
                        var subtitle = ReadSubtitle(input);
                        subtitle.PropertyChanged += SubtitlePropertyChanged;
                        result.Add(subtitle);
                    }
                    input.Seek(returnPos, SeekOrigin.Begin);
                }
            }

            LoadChanges(result);

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
                var offsets = new List<long>();

                var count1 = input.ReadInt32();
                output.Write(count1);
                output.Write(input.ReadBytes(4));

                long outputOffset = 0;

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
                        long offset2 = input.ReadInt32();
                        if (offset2 == 0)
                        {
                            offsets.Add(-1);
                        }
                        else 
                        {
                            offsets.Add(outputOffset);
                            outputOffset = WriteSubtitle(tempOutput, subtitles, offset2);
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
                        output.Write((int)(outputOffset + offset));
                    }
                }

                var data = tempMemoryStream.ToArray();
                output.Write(data);
            }
        }

        private long WriteSubtitle(ExtendedBinaryWriter output, IList<Subtitle> subtitles, long inputOffset)
        {
            var sub = subtitles.First(x => x.Offset == inputOffset);
            output.WriteString(sub.Translation);
            var result = (int) output.Position;

            return result;
        }
    }
}
