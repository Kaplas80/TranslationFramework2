using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using TF.Core.Entities;
using TF.Core.Helpers;
using TF.IO;
using WeifenLuo.WinFormsUI.Docking;

namespace TFGame.TheMissing.Files.Txt
{
    public class File : TranslationFile
    {
        [DllImport("msgconv4tm_dll_win.dll")]
        private static extern int MsgConvGetMsgInfoWithEnum([In] [Out] MsgInfo info, byte[] data, int msgEnum, bool voiceOnly = true);
        [DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont, IntPtr pdv, [In] ref uint pcFonts);

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

        private PrivateFontCollection _fontCollection;

        private readonly Font _cellFont;

        public override int SubtitleCount
        {
            get
            {
                var subtitles = GetSubtitles();
                return subtitles.Count(x => (x != null) && (!string.IsNullOrEmpty(x.Text)));
            }
        }

        public File(string path, string changesFolder, System.Text.Encoding encoding) : base(path, changesFolder, encoding)
        {
            Type = FileType.TextFile;

            _fontCollection = new PrivateFontCollection();
            var fontFile = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Path), "mplus-1m-regular.ttf");
            var font = System.IO.File.ReadAllBytes(fontFile);
            var fontPtr = Marshal.AllocCoTaskMem(font.Length);
            Marshal.Copy(font, 0, fontPtr, font.Length);
            uint dummy = 0;
            _fontCollection.AddMemoryFont(fontPtr, font.Length);
            AddFontMemResourceEx(fontPtr, (uint)font.Length, IntPtr.Zero, ref dummy);
            Marshal.FreeCoTaskMem(fontPtr);

            _cellFont = new Font(_fontCollection.Families[0], 20f, FontStyle.Regular, GraphicsUnit.Pixel);
        }

        public override void Open(DockPanel panel, ThemeBase theme)
        {
            _view = new GridView(theme);
            _view.AutoAdjustSizes += ViewOnAutoAdjustSizes;
            
            _subtitles = GetSubtitles();
            _view.LoadData(_subtitles.Where(x => !string.IsNullOrEmpty(x.Text)).ToList());
            _view.Show(panel, DockState.Document);
        }

        private void ViewOnAutoAdjustSizes()
        {
            foreach (var subtitle in _subtitles)
            {
                if (subtitle.Width >= 0 && subtitle.Height >= 0)
                {
                    var newSize = TextSize(subtitle.Translation);
                    subtitle.Width = newSize.Width;
                    subtitle.Height = newSize.Height;
                }
            }

            _view.Refresh();
        }

        private SizeF TextSize(string str)
        {
            var lines = 1;
            var maxWidth = 0f;
            var acum = 34f;
            if (str[0] == ' ')
            {
                acum = 43.5f;
            }

            var words = str.Split(' ');
            for (var i = 0; i < words.Length; i++)
            {
                var tempWidth = GetWordWidth(words[i], i < words.Length - 1);
                if (acum + tempWidth < 348.5)
                {
                    acum += tempWidth;
                    if (acum > maxWidth)
                    {
                        maxWidth = acum;
                    }
                }
                else
                {
                    acum = 34f + tempWidth;
                    lines++;
                }
            }

            var height = lines * 24 + 28;

            if (Path.EndsWith("msg0801en.txt") && height < 76)
            {
                height = 76;
            }

            return new SizeF((float) Math.Ceiling(maxWidth), height);
        }

        private float GetWordWidth(string word, bool addSpace)
        {
            var result = 0f;
            foreach (var chr in word)
            {
                result += GetCharWidth(chr);
            }

            if (addSpace)
            {
                result += 9.5f;
            }

            return result;
        }

        private float GetCharWidth(char chr)
        {
            SizeF size;

            using (var graphics = Graphics.FromHwnd(IntPtr.Zero))
            {
                size = graphics.MeasureString(chr.ToString(), _cellFont, int.MaxValue, StringFormat.GenericTypographic);
                size.Width = (int)Math.Ceiling(size.Width);
            }

            return size.Width <= 10f ? 9.5f : 19.5f;
        }

        protected virtual IList<Subtitle> GetSubtitles()
        {
            var result = new List<Subtitle>();

            var data = System.IO.File.ReadAllBytes(Path);
            var startMsg = int.Parse(System.IO.Path.GetFileName(Path).Substring(3, 4)) * 10000;
            var idDictionary = new Dictionary<string, int>();
            var srcEncoding = System.Text.Encoding.GetEncoding(1252);

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

            var sizeDictionary = ReadBoxSizes(System.IO.Path.GetDirectoryName(Path));

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
                    
                    if (sizeDictionary.ContainsKey(id))
                    {
                        var sz = sizeDictionary[id];
                        subtitle.Width = sz[1].Width;
                        subtitle.LoadedWidth = sz[1].Width;
                        subtitle.Height = sz[1].Height;
                        subtitle.LoadedHeight = sz[1].Height;
                    }
                    else
                    {
                        subtitle.Width = -1;
                        subtitle.LoadedWidth = -1;
                        subtitle.Height = -1;
                        subtitle.LoadedHeight = -1;
                    }
                    subtitle.PropertyChanged += SubtitlePropertyChanged;
                    result.Add(subtitle);
                }
            }

            LoadChanges(result);

            return result;
        }

        private static IDictionary<int, SizeF[]> ReadBoxSizes(string path)
        {
            var result = new Dictionary<int, SizeF[]>();
            var file = System.IO.Path.Combine(path, "resources_00001.-13");
            using (var fs = new FileStream(file, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fs))
            {
                input.BaseStream.Seek(0x150, SeekOrigin.Begin);

                var heightCount = input.ReadInt32();
                for (var i = 0; i < heightCount; i++)
                {
                    var nameLength = input.ReadInt32();
                    var name = input.ReadString();
                    var msgEnum = input.ReadInt32();
                    var langCount = input.ReadInt32();

                    var sizes = new SizeF[langCount];
                    for (var j = 0; j < langCount; j++)
                    {
                        var sz = new SizeF(input.ReadSingle(), input.ReadSingle());
                        sizes[j] = sz;
                    }
                    
                    result.Add(msgEnum, sizes);
                }
            }

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

            WriteBoxSizes(System.IO.Path.GetDirectoryName(outputPath), subtitles);
        }

        private static void WriteBoxSizes(string path, IList<Subtitle> subtitles)
        {
            var file = System.IO.Path.Combine(path, "resources_00001.-13");
            using (var fs = new FileStream(file, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fs))
            using (var output = new ExtendedBinaryWriter(fs))
            {
                input.Seek(0x150, SeekOrigin.Begin);

                var heightCount = input.ReadInt32();
                for (var i = 0; i < heightCount; i++)
                {
                    var nameLength = input.ReadInt32();
                    var name = input.ReadString();
                    var msgEnum = input.ReadInt32();
                    var langCount = input.ReadInt32();

                    input.Skip(8);

                    var sub = subtitles.FirstOrDefault(x => x.MsgId == msgEnum);

                    if (sub != null)
                    {
                        if (sub.Width > 0 && sub.Height > 0)
                        {
                            output.Write(sub.Width);
                            output.Write(sub.Height);
                        }
                    }
                    else
                    {
                        input.Skip(8);
                    }

                    input.Skip(16);
                }
            }
        }

        public override bool Search(string searchString)
        {
            var bytes = System.IO.File.ReadAllBytes(Path);

            var pattern = FileEncoding.GetBytes(searchString);

            var searchHelper = new SearchHelper(pattern);
            var index1 = searchHelper.Search(bytes);

            var index2 = -1;
            if (HasChanges)
            {
                bytes = System.IO.File.ReadAllBytes(ChangesFile);
                pattern = Encoding.Unicode.GetBytes(searchString);
                searchHelper = new SearchHelper(pattern);
                index2 = searchHelper.Search(bytes);
            }

            return index1 != -1 || index2 != -1;
        }

        public override void SaveChanges()
        {
            using (var fs = new FileStream(ChangesFile, FileMode.Create))
            using (var output = new ExtendedBinaryWriter(fs, System.Text.Encoding.Unicode))
            {
                output.Write(ChangesFileVersion);
                output.Write(_subtitles.Count);
                foreach (var subtitle in _subtitles)
                {
                    output.Write(subtitle.Offset);
                    output.WriteString(subtitle.Translation);
                    output.Write(subtitle.Width);
                    output.Write(subtitle.Height);
                    subtitle.Loaded = subtitle.Translation;
                    subtitle.LoadedWidth = subtitle.Width;
                    subtitle.LoadedHeight = subtitle.Height;
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
                using (var input = new ExtendedBinaryReader(fs, System.Text.Encoding.Unicode))
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
                        var width = input.ReadSingle();
                        var height = input.ReadSingle();

                        var subtitle = subtitles.FirstOrDefault(x => x.Offset == offset);
                        if (subtitle != null)
                        {
                            subtitle.PropertyChanged -= SubtitlePropertyChanged;
                            subtitle.Translation = text;
                            subtitle.Width = width;
                            subtitle.Height = height;
                            subtitle.Loaded = subtitle.Translation;
                            subtitle.LoadedWidth = subtitle.Width;
                            subtitle.LoadedHeight = subtitle.Height;
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
