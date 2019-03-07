using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using TF.Core.Exceptions;
using TF.IO;
using WeifenLuo.WinFormsUI.Docking;
using YakuzaCommon.Files.SimpleSubtitle;

namespace YakuzaCommon.Files.Exe
{
    public class File : SimpleSubtitle.File
    {
        private CharacterInfo[] _data;
        private FontTableView _ftview;

        protected virtual long FontTableOffset => 0;
        public override int SubtitleCount => 1;

        protected File(string path, string changesFolder, Encoding encoding) : base(path, changesFolder, encoding)
        {
        }

        public override void Open(DockPanel panel, ThemeBase theme)
        {
            _view = new View(theme);
            _subtitles = GetSubtitles();
            _view.LoadSubtitles(_subtitles.Where(x => !string.IsNullOrEmpty(x.Text)).ToList());
            _view.Show(panel, DockState.Document);

            _ftview = new FontTableView(theme);
            _data = GetFontTable();
            _ftview.LoadFontTable(_data);
            _ftview.Show(panel, DockState.Document);
        }

        protected virtual CharacterInfo[] GetFontTable()
        {
            var result = new CharacterInfo[256];

            using (var fs = new FileStream(Path, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fs, FileEncoding, Endianness.LittleEndian))
            {
                input.Seek(FontTableOffset, SeekOrigin.Begin);

                for (var i = 0; i < 256; i++)
                {
                    var tl = input.ReadSingle();
                    var tr = input.ReadSingle();
                    var ml = input.ReadSingle();
                    var mr = input.ReadSingle();
                    var bl = input.ReadSingle();
                    var br = input.ReadSingle();
                    result[i] = new CharacterInfo((byte) i)
                    {
                        OriginalData =
                        {
                            [0] = tl,
                            [1] = tr,
                            [2] = ml,
                            [3] = mr,
                            [4] = bl,
                            [5] = br
                        },
                        Data =
                        {
                            [0] = tl,
                            [1] = tr,
                            [2] = ml,
                            [3] = mr,
                            [4] = bl,
                            [5] = br
                        }
                    };

                }
            }

            if (HasChanges)
            {
                try
                {
                    LoadFontTableChanges(ChangesFile, result);
                }
                catch (ChangesFileVersionMismatchException e)
                {
                    System.IO.File.Delete(ChangesFile);
                }
            }

            for (var i = 0; i < 256; i++)
            {
                result[i].SetLoadedData();
                result[i].PropertyChanged += SubtitlePropertyChanged;
            }

            return result;
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

            return result;
        }

        public override void SaveChanges()
        {
            using (var fs = new FileStream(ChangesFile, FileMode.Create))
            using (var output = new ExtendedBinaryWriter(fs, Encoding.Unicode))
            {
                output.Write(ChangesFileVersion);

                for (var i = 0; i < 256; i++)
                {
                    output.Write(_data[i][0]);
                    output.Write(_data[i][1]);
                    output.Write(_data[i][2]);
                    output.Write(_data[i][3]);
                    output.Write(_data[i][4]);
                    output.Write(_data[i][5]);

                    _data[i].SetLoadedData();
                }

                // Save subtitles
            }

            NeedSaving = false;
            OnFileChanged();
        }

        protected override IList<Subtitle> LoadChanges(string file)
        {
            return new List<Subtitle>();
        }

        private void LoadFontTableChanges(string file, CharacterInfo[] data)
        {
            using (var fs = new FileStream(file, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fs, Encoding.Unicode))
            {
                var version = input.ReadInt32();

                if (version != ChangesFileVersion)
                {
                    throw new ChangesFileVersionMismatchException();
                }

                for (var i = 0; i < 256; i++)
                {
                    data[i][0] = input.ReadSingle();
                    data[i][1] = input.ReadSingle();
                    data[i][2] = input.ReadSingle();
                    data[i][3] = input.ReadSingle();
                    data[i][4] = input.ReadSingle();
                    data[i][5] = input.ReadSingle();
                }
            }
        }

        protected override void SubtitlePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            NeedSaving = _subtitles.Any(subtitle => subtitle.Loaded != subtitle.Translation) || _data.Any(x => x.HasChanged);
            OnFileChanged();
        }

        public override void Rebuild(string outputFolder)
        {
            var outputPath = System.IO.Path.Combine(outputFolder, RelativePath);
            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(outputPath));
            System.IO.File.Copy(Path, outputPath, true);

            RebuildSubtitles(outputPath);
            RebuildFontTable(outputPath);
        }

        private void RebuildSubtitles(string outputFile)
        {

        }

        private void RebuildFontTable(string outputFile)
        {
            var data = GetFontTable();

            using (var fs = new FileStream(outputFile, FileMode.Open))
            using (var output = new ExtendedBinaryWriter(fs, FileEncoding, Endianness.LittleEndian))
            {
                output.Seek(FontTableOffset, SeekOrigin.Begin);

                for (var i = 0; i < 256; i++)
                {
                    output.Write(data[i][0]);
                    output.Write(data[i][1]);
                    output.Write(data[i][2]);
                    output.Write(data[i][3]);
                    output.Write(data[i][4]);
                    output.Write(data[i][5]);
                }
            }
        }
    }
}
