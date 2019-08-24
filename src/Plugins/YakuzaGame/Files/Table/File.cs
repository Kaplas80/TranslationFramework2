using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using TF.Core.Entities;
using TF.Core.Helpers;
using TF.IO;
using WeifenLuo.WinFormsUI.Docking;

namespace YakuzaGame.Files.Table
{
    public class File : TranslationFile
    {
        private View _view;
        private TableData _data;

        public override int SubtitleCount
        {
            get
            {
                var data = GetData();
                return data.NumRows;
            }
        }

        public File(string path, string changesFolder, Encoding encoding = null) : base(path, changesFolder, encoding)
        {
            Type = FileType.TextFile;
        }

        public override void Open(DockPanel panel, ThemeBase theme)
        {
            _view = new View(theme);

            _data = GetData();

            _view.LoadData(_data);
            _view.Show(panel, DockState.Document);
        }

        private TableData GetData()
        {
            var result = new TableData();

            using (var fs = new FileStream(Path, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fs, FileEncoding, Endianness.BigEndian))
            {
                var signature = input.ReadInt32();
                if (signature != 0x20070319)
                {
                    return result;
                }

                var numColumns = input.ReadInt32();
                var numRows = input.ReadInt32();

                result.NumRows = numRows;

                input.Skip(4);

                for (var i = 0; i < numColumns; i++)
                {
                    input.Seek(16 + i * 64, SeekOrigin.Begin);
                    var columnName = input.ReadString(Encoding.UTF8);
                    input.Seek(16 + i * 64 + 48, SeekOrigin.Begin);
                    var type = input.ReadInt32();
                    var dataCount = input.ReadInt32();
                    var sectionSize = input.ReadInt32();
                    input.Skip(4);

                    var column = ColumnFactory.GetColumn(type, numRows);

                    column.Name = columnName;
                    column.DataCount = dataCount;
                    column.Size = sectionSize;

                    column.PropertyChanged += SubtitlePropertyChanged;

                    result.Columns.Add(column);
                }

                for (var i = 0; i < numColumns; i++)
                {
                    var column = result.Columns[i];
                    column.ReadData(input);
                }
            }

            LoadChanges(result);

            return result;
        }

        protected virtual void SubtitlePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!_data.Columns.Any(column => column.HasChanges))
            {
                return;
            }

            NeedSaving = true;
            OnFileChanged();
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
            using (var output = new ExtendedBinaryWriter(fs, Encoding.Unicode))
            {
                output.Write(ChangesFileVersion);
                foreach (var column in _data.Columns)
                {
                    column.SaveChanges(output);
                }
            }

            NeedSaving = false;
            OnFileChanged();
        }

        protected virtual void LoadChanges(TableData data)
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

                    foreach (var column in data.Columns)
                    {
                        column.PropertyChanged -= SubtitlePropertyChanged;
                        column.LoadChanges(input);
                        column.PropertyChanged += SubtitlePropertyChanged;
                    }
                }
            }
        }

        public override void Rebuild(string outputFolder)
        {
            var outputPath = System.IO.Path.Combine(outputFolder, RelativePath);
            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(outputPath));

            var data = GetData();
            using (var fsInput = new FileStream(Path, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fsInput, FileEncoding, Endianness.BigEndian))
            using (var fsOutput = new FileStream(outputPath, FileMode.Create))
            using (var output = new ExtendedBinaryWriter(fsOutput, FileEncoding, Endianness.BigEndian))
            {
                output.Write(input.ReadBytes(4));

                var numColumns = input.ReadInt32();
                var numRows = input.ReadInt32();

                output.Write(numColumns);
                output.Write(numRows);
                output.Write(input.ReadBytes(4));

                foreach (var column in data.Columns)
                {
                    column.WriteInfo(output);
                }

                foreach (var column in data.Columns)
                {
                    column.WriteData(output);
                }
            }
        }

        public override bool SearchText(string searchString, int direction)
        {
            if (_data == null || _data.NumRows == 0)
            {
                return false;
            }

            int i;
            if (direction == 0)
            {
                i = 1;
            }
            else
            {
                i = _view.GetSelectedRow() + direction;
            }

            var step = direction < 0 ? -1 : 1;

            var result = -1;
            while (i >= 0 && i < _data.NumRows && result == -1)
            {
                foreach (var column in _data.Columns)
                {
                    var original = column.GetValue(i);
                    var translation = column.GetTranslatedValue(i);

                    if (!string.IsNullOrEmpty(original))
                    {
                        if (original.Contains(searchString) || (!string.IsNullOrEmpty(translation) && translation.Contains(searchString)))
                        {
                            result = i;
                            break;
                        }
                    }
                }

                i += step;
            }

            if (result != -1)
            {
                _view.DisplaySubtitle(result);
            }

            return result != -1;
        }
    }
}
