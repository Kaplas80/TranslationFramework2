using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using TF.Core.TranslationEntities;
using TF.IO;
using WeifenLuo.WinFormsUI.Docking;

namespace YakuzaGame.Files.Exe
{
    public class File : PEFile
    {
        private CharacterInfo[] _data;
        private FontTableView _ftView;
        private PatchView _patchView;
        private List<ExePatch> _patches;

        protected virtual long FontTableOffset => 0;
        protected virtual List<ExePatch> Patches => new List<ExePatch>();

        protected File(string path, string changesFolder, Encoding encoding) : base(path, changesFolder, encoding)
        {
        }

        public override void Open(DockPanel panel)
        {
            _patchView = new PatchView();
            _patches = GetPatches();
            _patchView.LoadPatches(_patches);
            _patchView.Show(panel, DockState.Document);

            _ftView = new FontTableView();
            _data = GetFontTable();
            _ftView.LoadFontTable(_data);
            _ftView.Show(panel, DockState.Document);

            base.Open(panel);
        }

        protected virtual List<ExePatch> GetPatches()
        {
            var result = new List<ExePatch>();
            foreach (var exePatch in Patches)
            {
                result.Add(exePatch);
            }

            LoadPatchesChanges(result);

            foreach (var exePatch in result)
            {
                exePatch.LoadedStatus = exePatch.Enabled;
                exePatch.PropertyChanged += SubtitlePropertyChanged;
            }

            return result;
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

            LoadFontTableChanges(result);

            for (var i = 0; i < 256; i++)
            {
                result[i].SetLoadedData();
                result[i].PropertyChanged += SubtitlePropertyChanged;
            }

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

                foreach (var patch in _patches)
                {
                    output.Write(patch.Enabled ? 1 : 0);
                    patch.LoadedStatus = patch.Enabled;
                }

                output.Write(_subtitles.Count);
                foreach (var subtitle in _subtitles)
                {
                    output.Write(subtitle.Offset);
                    output.WriteString(subtitle.Translation);

                    subtitle.Loaded = subtitle.Translation;
                }
            }

            NeedSaving = false;
            OnFileChanged();
        }

        protected override void LoadChanges(IList<Subtitle> subtitles)
        {
            if (HasChanges)
            {
                using (var fs = new FileStream(ChangesFile, FileMode.Open))
                using (var input = new ExtendedBinaryReader(fs, Encoding.Unicode))
                {
                    var version = input.ReadInt32();

                    if (version != ChangesFileVersion)
                    {
                        //System.IO.File.Delete(ChangesFile);
                        return;
                    }

                    input.Skip(256 * 6 * 4);

                    input.Skip(Patches.Count * 4);

                    var subtitleCount = input.ReadInt32();

                    for (var i = 0; i < subtitleCount; i++)
                    {
                        var offset = input.ReadInt64();
                        var text = input.ReadString();

                        var subtitle = subtitles.FirstOrDefault(x => x.Offset == offset);
                        if (subtitle != null)
                        {
                            subtitle.PropertyChanged -= SubtitlePropertyChanged;
                            subtitle.Translation = text;
                            subtitle.Loaded = subtitle.Translation;
                            subtitle.PropertyChanged += SubtitlePropertyChanged;
                        }
                    }
                }
            }
        }

        private void LoadFontTableChanges(CharacterInfo[] data)
        {
            if (HasChanges)
            {
                using (var fs = new FileStream(ChangesFile, FileMode.Open))
                using (var input = new ExtendedBinaryReader(fs, Encoding.Unicode))
                {
                    var version = input.ReadInt32();

                    if (version != ChangesFileVersion)
                    {
                        //System.IO.File.Delete(ChangesFile);
                        return;
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
        }

        private void LoadPatchesChanges(IList<ExePatch> data)
        {
            if (HasChanges)
            {
                using (var fs = new FileStream(ChangesFile, FileMode.Open))
                using (var input = new ExtendedBinaryReader(fs, Encoding.Unicode))
                {
                    var version = input.ReadInt32();

                    if (version != ChangesFileVersion)
                    {
                        //System.IO.File.Delete(ChangesFile);
                        return;
                    }

                    input.Skip(256 * 6 * 4);

                    foreach (var patch in data)
                    {
                        var value = input.ReadInt32();
                        patch.Enabled = value == 1;
                    }
                }
            }
        }

        protected override void SubtitlePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            NeedSaving = _subtitles.Any(subtitle => subtitle.HasChanges) || _data.Any(x => x.HasChanged) || _patches.Any(y => y.HasChanges);
            OnFileChanged();
        }

        public override void Rebuild(string outputFolder)
        {
            var outputPath = System.IO.Path.GetFullPath(System.IO.Path.Combine(outputFolder, RelativePath));
            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(outputPath));

            RebuildSubtitles(outputPath);
            RebuildFontTable(outputPath);
            RebuildPatches(outputPath);
        }

        private void RebuildFontTable(string outputFile)
        {
            if (!System.IO.File.Exists(outputFile))
            {
                System.IO.File.Copy(Path, outputFile);
            }

            var data = GetFontTable();

            using (var fs = new FileStream(outputFile, FileMode.Open))
            using (var output = new ExtendedBinaryWriter(fs, FileEncoding))
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

        private void RebuildPatches(string outputFile)
        {
            if (!System.IO.File.Exists(outputFile))
            {
                System.IO.File.Copy(Path, outputFile);
            }

            var data = GetPatches();

            using (var fs = new FileStream(outputFile, FileMode.Open))
            using (var output = new ExtendedBinaryWriter(fs, FileEncoding))
            {
                foreach (var patch in data)
                {
                    if (patch.Enabled)
                    {
                        foreach (var patchInfo in patch.Patches)
                        {
                            output.Seek(patchInfo.Item1, SeekOrigin.Begin);
                            output.Write(patchInfo.Item2);
                        }
                    }
                }
            }
        }
    }
}
