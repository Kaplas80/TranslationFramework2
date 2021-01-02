using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TF.Core.Entities;
using TF.Core.Helpers;
using TF.IO;
using WeifenLuo.WinFormsUI.Docking;
using Yarhl.IO;
using Yarhl.Media.Text;

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

        public File(string gameName, string path, string changesFolder, System.Text.Encoding encoding = null) : base(gameName, path, changesFolder, encoding)
        {
            Type = FileType.TextFile;
        }

        public override void Open(DockPanel panel)
        {
            _view = new View();

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

        public override bool Search(string searchString, string path = "")
        {
            if (string.IsNullOrEmpty(path))
            {
                path = Path;
            }
            var bytes = System.IO.File.ReadAllBytes(path);

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

        public override void ExportPo(string path)
        {
            var directory = System.IO.Path.GetDirectoryName(path);
            Directory.CreateDirectory(directory);

            var po = new Po()
            {
                Header = new PoHeader(GameName, "dummy@dummy.com", "es-ES")
            };

            var data = GetData();

            foreach (var dataColumn in data.Columns)
            {
                if (dataColumn.GetType().Name != nameof(Column) && dataColumn.DataCount > 0 && dataColumn.Size > 0)
                {
                    var values = dataColumn.GetUniqueValues();
                    for (var i = 0; i < values.Count; i++)
                    {
                        var value = values[i];
                        var original = value.Item1;
                        var translated = value.Item2;
                        var entry = new PoEntry();
                        
                        if (string.IsNullOrEmpty(original))
                        {
                            original = "<!empty>";
                            translated = "<!empty>";
                        }

                        if (string.IsNullOrEmpty(translated))
                        {
                            translated = "<!empty>";
                        }

                        var tmp = original.Replace(LineEnding.ShownLineEnding, LineEnding.PoLineEnding);
                        
                        entry.Original = tmp;
                        entry.Context = $"{dataColumn.Name}_{i}";

                        if (original != translated)
                        {
                            tmp = translated.Replace(LineEnding.ShownLineEnding, LineEnding.PoLineEnding);
                            entry.Translated = tmp;
                        }

                        po.Add(entry);
                    }
                }
            }

            var po2binary = new Yarhl.Media.Text.Po2Binary();
            var binary = po2binary.Convert(po);

            binary.Stream.WriteTo(path);
        }

        public override void ImportPo(string inputFile, bool save = true, bool parallel = true)
        {
            var dataStream = DataStreamFactory.FromFile(inputFile, FileOpenMode.Read);
            var binary = new BinaryFormat(dataStream);
            var binary2Po = new Yarhl.Media.Text.Po2Binary();
            var po = binary2Po.Convert(binary);

            LoadBeforeImport();
            foreach (var dataColumn in _data.Columns)
            {
                if (dataColumn.GetType().Name != nameof(Column) && dataColumn.DataCount > 0 && dataColumn.Size > 0)
                {
                    var values = dataColumn.GetUniqueValues();
                    var newValues = new Dictionary<string, string>();
                    for (var i = 0; i < values.Count; i++)
                    {
                        var value = values[i];
                        var original = value.Item1;
                        
                        if (string.IsNullOrEmpty(original))
                        {
                            original = "<!empty>";
                        }

                        var tmp = original.Replace(LineEnding.ShownLineEnding, LineEnding.PoLineEnding);
                        var entry = po.FindEntry(tmp, $"{dataColumn.Name}_{i}");

                        if (entry.Text == "<!empty>")
                        {
                            newValues.Add($"{dataColumn.Name}|{i}", value.Item1); 
                        }
                        else
                        {
                            var tmp1 = entry.Translated;
                            tmp1 = string.IsNullOrEmpty(tmp1) ? value.Item1 : tmp1.Replace(LineEnding.PoLineEnding, LineEnding.ShownLineEnding);
                            newValues.Add($"{dataColumn.Name}|{i}", tmp1); 
                        }
                    }

                    dataColumn.SetUniqueValues(newValues, true);
                }
            }

            if (save && NeedSaving)
            {
                SaveChanges();
            }
        }

        protected override void LoadBeforeImport()
        {
            _data = GetData();
        }
    }
}
