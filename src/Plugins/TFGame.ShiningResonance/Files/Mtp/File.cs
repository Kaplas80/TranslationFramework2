using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TF.Core.Entities;
using TF.Core.Files;
using TF.Core.TranslationEntities;
using TF.IO;

namespace TFGame.ShiningResonance.Files.Mtp
{
    class File : BinaryTextFile
    {
        public File(string path, string changesFolder, System.Text.Encoding encoding) : base(path, changesFolder, encoding)
        {
        }

        protected override IList<Subtitle> GetSubtitles()
        {
            var result = new List<Subtitle>();

            using (var fs = new FileStream(Path, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fs, FileEncoding))
            {
                input.Skip(0x20); // header

                input.Skip(4);
                var stringCount1 = input.ReadInt32();
                var aux = input.ReadInt32();
                var stringCount2 = input.ReadInt32();
                input.Skip(4);

                if (aux == 3)
                {
                    input.Skip(4);
                }

                var language = input.ReadInt32();

                var base1 = input.Position;
                var base2 = base1 + 4 * stringCount1;
                var stringsBase = base2 + aux * 4 * stringCount1;

                for (var i = 0; i < stringCount1; i++)
                {
                    input.Seek(base1 + 4 * i, SeekOrigin.Begin);
                    var index1 = input.ReadInt32();

                    input.Seek(base2 + 4 * index1, SeekOrigin.Begin);
                    input.Skip(4);

                    if (aux == 3)
                    {
                        input.Skip(4);
                    }

                    var strOffset = input.ReadInt32();
                    input.Seek(stringsBase + strOffset, SeekOrigin.Begin);
                    input.Skip(4);

                    var subtitle = ReadSubtitle(input);
                    subtitle.PropertyChanged += SubtitlePropertyChanged;

                    result.Add(subtitle);
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
            using (var input = new ExtendedBinaryReader(fsInput, FileEncoding))
            using (var fsOutput = new FileStream(outputPath, FileMode.Create))
            using (var output = new ExtendedBinaryWriter(fsOutput, FileEncoding))
            {
                output.Write(input.ReadBytes(0x20)); // Escribo la cabecera, aunque al final tendré que editarla

                output.Write(input.ReadInt32());

                var stringCount1 = input.ReadInt32();
                output.Write(stringCount1);
                var aux = input.ReadInt32();
                output.Write(aux);
                var stringCount2 = input.ReadInt32();
                output.Write(stringCount2);

                output.Write(input.ReadInt32());

                if (aux == 3)
                {
                    output.Write(input.ReadInt32());
                }

                var language = input.ReadInt32();
                output.Write(language);

                var base1 = input.Position;
                var base2 = base1 + 4 * stringCount1;
                var stringsBase = base2 + aux * 4 * stringCount1;

                var outputOffset = 0;

                for (var i = 0; i < stringCount1; i++)
                {
                    input.Seek(base1 + 4 * i, SeekOrigin.Begin);
                    output.Seek(base1 + 4 * i, SeekOrigin.Begin);
                    var index1 = input.ReadInt32();
                    output.Write(index1);

                    input.Seek(base2 + 4 * index1, SeekOrigin.Begin);
                    output.Seek(base2 + 4 * index1, SeekOrigin.Begin);
                    output.Write(input.ReadInt32());

                    if (aux == 3)
                    {
                        output.Write(input.ReadInt32());
                    }

                    var strOffset = input.ReadInt32();
                    output.Write(outputOffset);

                    input.Seek(stringsBase + strOffset, SeekOrigin.Begin);
                    output.Seek(stringsBase + outputOffset, SeekOrigin.Begin);

                    var originalSize = input.ReadInt32();
                    var subtitle = ReadSubtitle(input);
                    
                    var outputSubtitle = subtitles.FirstOrDefault(x => x.Offset == subtitle.Offset);

                    if (outputSubtitle != null)
                    {
                        var translationSize = FileEncoding.GetByteCount(outputSubtitle.Translation);
                        output.Write((int) translationSize);
                        WriteSubtitle(output, outputSubtitle, output.Position, false);
                    }
                    else
                    {
                        output.Write(originalSize);
                        WriteSubtitle(output, subtitle, output.Position, false);
                    }

                    output.WritePadding(0x04);

                    outputOffset = (int) (output.Position - stringsBase);
                }

                output.WritePadding(0x10);

                var size = output.Position - 0x20;

                input.Seek(-0x80, SeekOrigin.End);
                output.Write(input.ReadBytes(0x80));

                output.Seek(0x04, SeekOrigin.Begin);
                output.Write((int)(size + 0x60));
                output.Skip(12);
                output.Write((int)size);
            }
        }
    }
}
