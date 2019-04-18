using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using TF.Core.Entities;
using TF.Core.Files;
using TF.Core.Helpers;
using TF.Core.Views;
using TF.IO;
using WeifenLuo.WinFormsUI.Docking;

namespace TFGame.TheMissing.Files.Txt
{
    public class File : TranslationFile
    {
        [DllImport("msgconv4tm_dll_win.dll")]
        private static extern int MsgConvGetMsgInfoWithEnum([In] [Out] MsgInfo info, byte[] data, int msgEnum, bool voiceOnly = true);

        [StructLayout(LayoutKind.Sequential)]
        private class MsgInfo
        {
            public int EnumMsg;
            public int FrameTotal;
            public int NoName;
            public int NoEmotion;
            public int Cmd;
            public int Prm0;
            public int Prm1;
            public IntPtr Voice;
            public string Text;
        }

        protected IList<Subtitle> _subtitles;

        protected GridView _view;

        public File(string path, string changesFolder, Encoding encoding) : base(path, changesFolder, encoding)
        {
        }

        public override void Open(DockPanel panel, ThemeBase theme)
        {
            _view = new GridView(theme);

            _subtitles = GetSubtitles();
            _view.LoadData(_subtitles.Where(x => !string.IsNullOrEmpty(x.Text)).ToList());
            _view.Show(panel, DockState.Document);
        }

        protected virtual IList<Subtitle> GetSubtitles()
        {
            var result = new List<Subtitle>();
            var data = System.IO.File.ReadAllBytes(Path);
            var startMsg = int.Parse(System.IO.Path.GetFileName(Path).Substring(3, 4)) * 10000;
            var idDictionary = new Dictionary<string, int>();
            var srcEncoding = Encoding.GetEncoding(1252);

            for (var i = 0; i < 10000; i++)
            {
                var info = new MsgInfo();
                var res = MsgConvGetMsgInfoWithEnum(info, data, startMsg + i, true);
                if (res == 2 && !string.IsNullOrEmpty(info.Text))
                {
                    var ansiBytes = srcEncoding.GetBytes(info.Text);
                    var txt = FileEncoding.GetString(ansiBytes);
                    if (!idDictionary.ContainsKey(txt))
                    {
                        idDictionary.Add(txt, info.EnumMsg);
                    }
                }
            }

            using (var fs = new FileStream(Path, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fs, FileEncoding))
            {
                input.Seek(0x10, SeekOrigin.Begin);
                var stringOffsetTable = input.ReadInt32();
                var stringBase = input.ReadInt32();

                input.Seek(0x30, SeekOrigin.Begin);
                var stringCount = input.ReadInt32();

                for (var i = 0; i < stringCount; i++)
                {
                    input.Seek(stringOffsetTable + 4 * i, SeekOrigin.Begin);
                    var offset = input.ReadInt32();
                    input.Seek(stringBase + offset, SeekOrigin.Begin);
                    var sub = ReadSubtitle(input);

                    var id = -1;
                    if (idDictionary.ContainsKey(sub.Text))
                    {
                        id = idDictionary[sub.Text];
                    }

                    var subtitle = new Subtitle(sub, id);
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
                input.Seek(0x0C, SeekOrigin.Begin);
                var lengthTableOffset = input.ReadInt32();
                var stringOffsetTable = input.ReadInt32();
                var stringBase = input.ReadInt32();
                var stringOffsetTable2 = input.ReadInt32();
                var stringBase2 = input.ReadInt32();

                input.Seek(0x2C, SeekOrigin.Begin);
                var lengthCount = input.ReadInt32();
                var stringCount = input.ReadInt32();
                var stringCount2 = input.ReadInt32();

                input.Seek(0, SeekOrigin.Begin);
                output.Write(input.ReadBytes(stringOffsetTable));

                long outputOffset = stringBase;
                var lengths = new Dictionary<int, short>();

                for (var i = 0; i < stringCount; i++)
                {
                    input.Seek(stringOffsetTable + 4 * i, SeekOrigin.Begin);
                    var offset = input.ReadInt32();

                    output.Seek(stringOffsetTable + 4 * i, SeekOrigin.Begin);
                    output.Write((int)(outputOffset - stringBase));

                    output.Seek(outputOffset, SeekOrigin.Begin);
                    var newOutputOffset = WriteSubtitle(output, subtitles, stringBase + offset, outputOffset);
                    var length = newOutputOffset - outputOffset - 1;
                    lengths.Add(i, (short)length);
                    outputOffset = newOutputOffset;
                }

                output.WritePadding(0x10);

                var newStringBase2 = output.Position;

                input.Seek(stringOffsetTable2, SeekOrigin.Begin);
                output.Seek(stringOffsetTable2, SeekOrigin.Begin);
                output.Write(input.ReadBytes(stringCount2 * 4));

                input.Seek(stringBase2, SeekOrigin.Begin);
                output.Seek(newStringBase2, SeekOrigin.Begin);
                output.Write(input.ReadBytes((int) (input.Length - stringBase2)));

                output.Seek(0x1C, SeekOrigin.Begin);
                output.Write((int)newStringBase2);

                for (var i = 0; i < lengthCount; i++)
                {
                    input.Seek(lengthTableOffset + i * 16, SeekOrigin.Begin);
                    var id = input.ReadInt32();
                    output.Seek(lengthTableOffset + i * 16 + 4, SeekOrigin.Begin);
                    output.Write(lengths[id]);
                }
            }
        }

        public override bool Search(string searchString)
        {
            var bytes = System.IO.File.ReadAllBytes(Path);

            var pattern = FileEncoding.GetBytes(searchString);

            var index1 = SearchHelper.SearchPattern(bytes, pattern, 0);

            var index2 = -1;
            if (HasChanges)
            {
                bytes = System.IO.File.ReadAllBytes(ChangesFile);
                pattern = Encoding.Unicode.GetBytes(searchString);
                index2 = SearchHelper.SearchPattern(bytes, pattern, 0);
            }

            return index1 != -1 || index2 != -1;
        }

        public override void SaveChanges()
        {
            using (var fs = new FileStream(ChangesFile, FileMode.Create))
            using (var output = new ExtendedBinaryWriter(fs, Encoding.Unicode))
            {
                output.Write(ChangesFileVersion);
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

        protected virtual void LoadChanges(IList<Subtitle> subtitles)
        {
            if (HasChanges)
            {
                using (var fs = new FileStream(ChangesFile, FileMode.Open))
                using (var input = new ExtendedBinaryReader(fs, Encoding.Unicode))
                {
                    var version = input.ReadInt32();

                    if (version != ChangesFileVersion)
                    {
                        //File.Delete(ChangesFile);
                        return;
                    }

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

        public override bool SearchText(string searchString, int direction)
        {
            if (_subtitles == null || _subtitles.Count == 0)
            {
                return false;
            }

            int i;
            int rowIndex;
            if (direction == 0)
            {
                i = 1;
                rowIndex = 1;
            }
            else
            {
                var (item1, item2) = _view.GetSelectedSubtitle();
                i = _subtitles.IndexOf(item2) + direction;
                rowIndex = item1 + direction;
            }

            var step = direction < 0 ? -1 : 1;

            var result = -1;
            while (i >= 0 && i < _subtitles.Count)
            {
                var subtitle = _subtitles[i];
                var original = subtitle.Text;
                var translation = subtitle.Translation;

                if (!string.IsNullOrEmpty(original))
                {
                    if (original.Contains(searchString) || (!string.IsNullOrEmpty(translation) && translation.Contains(searchString)))
                    {
                        result = rowIndex;
                        break;
                    }

                    rowIndex += step;
                }

                i += step;
            }

            if (result != -1)
            {
                _view.DisplaySubtitle(rowIndex);
            }

            return result != -1;
        }

        protected virtual void SubtitlePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            NeedSaving = _subtitles.Any(subtitle => subtitle.HasChanges);
            OnFileChanged();
        }

        protected virtual TF.Core.TranslationEntities.Subtitle ReadSubtitle(ExtendedBinaryReader input)
        {
            var offset = input.Position;
            var subtitle = ReadSubtitle(input, offset, false);
            return subtitle;
        }

        protected virtual TF.Core.TranslationEntities.Subtitle ReadSubtitle(ExtendedBinaryReader input, long offset, bool returnToPos)
        {
            var pos = input.Position;

            input.Seek(offset, SeekOrigin.Begin);

            var text = input.ReadString();

            var subtitle = new TF.Core.TranslationEntities.Subtitle
            {
                Offset = offset,
                Text = text,
                Translation = text,
                Loaded = text,
            };

            if (returnToPos)
            {
                input.Seek(pos, SeekOrigin.Begin);
            }

            return subtitle;
        }

        protected virtual long WriteSubtitle(ExtendedBinaryWriter output, IList<Subtitle> subtitles, long inputOffset, long outputOffset)
        {
            var sub = subtitles.First(x => x.Offset == inputOffset);
            var result = WriteSubtitle(output, sub, outputOffset, false);

            return result;
        }

        protected virtual long WriteSubtitle(ExtendedBinaryWriter output, Subtitle subtitle, long offset, bool returnToPos)
        {
            var pos = output.Position;
            output.Seek(offset, SeekOrigin.Begin);
            output.WriteString(subtitle.Translation);

            var result = output.Position;

            if (returnToPos)
            {
                output.Seek(pos, SeekOrigin.Begin);
            }

            return result;
        }
    }
}
