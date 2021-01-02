﻿using System.Collections.Generic;
using System.IO;
using System.Text;
using TF.Core.Files;
using TF.Core.TranslationEntities;
using TF.IO;

namespace YakuzaGame.Files.Blacksmith
{
    public class File : BinaryTextFileWithOffsetTable
    {
        public File(string gameName, string path, string changesFolder, System.Text.Encoding encoding) : base(gameName, path, changesFolder, encoding)
        {
        }

        protected override IList<Subtitle> GetSubtitles()
        {
            var result = new List<Subtitle>();

            using (var fs = new FileStream(Path, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fs, FileEncoding, Endianness.BigEndian))
            {
                var tableEnd = input.ReadInt32();
                input.Skip(4);

                while (input.Position < tableEnd)
                {
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
            using (var input = new ExtendedBinaryReader(fsInput, FileEncoding, Endianness.BigEndian))
            using (var fsOutput = new FileStream(outputPath, FileMode.Create))
            using (var output = new ExtendedBinaryWriter(fsOutput, FileEncoding, Endianness.BigEndian))
            {
                var tableEnd = input.ReadInt32();
                output.Write(tableEnd);
                output.Write(input.ReadInt32());

                long outputOffset = input.PeekInt32();
                var firstStringOffset = outputOffset;

                while (input.Position < tableEnd)
                {
                    var offset = input.ReadInt32();
                    outputOffset = WriteSubtitle(output, subtitles, offset, outputOffset);
                }
                
                output.Write(input.ReadBytes((int) (firstStringOffset - tableEnd)));
            }
        }
    }
}
