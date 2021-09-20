using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using ExcelDataReader;
using OfficeOpenXml;
using TF.Core.Entities;
using TF.Core.Helpers;
using TF.Core.TranslationEntities;
using TF.Core.Views;
using TF.IO;
using WeifenLuo.WinFormsUI.Docking;
using Yarhl.IO;
using Yarhl.Media.Text;

namespace TF.Core.Files
{
    using System.Collections.Concurrent;
    using System.Threading.Tasks;

    public class BinaryTextFile : TranslationFile
    {
        protected virtual int StartOffset => 0;

        protected IList<Subtitle> _subtitles;

        protected GridView _view;

        public override int SubtitleCount
        {
            get
            {
                var subtitles = GetSubtitles();
                return subtitles.Count(x => (x != null) && (!string.IsNullOrEmpty(x.Text)));
            }
        }

        public BinaryTextFile(string gameName, string path, string changesFolder, System.Text.Encoding encoding) : base(gameName, path, changesFolder, encoding)
        {
            Type = FileType.TextFile;
        }

        public override void Open(DockPanel panel)
        {
            _view = new GridView(this);

            _subtitles = GetSubtitles();
            _view.LoadData(_subtitles.Where(x => !string.IsNullOrEmpty(x.Text)).ToList());
            _view.Show(panel, DockState.Document);
        }

        protected virtual IList<Subtitle> GetSubtitles()
        {
            var result = new List<Subtitle>();

            using (var fs = new FileStream(Path, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fs, FileEncoding))
            {
                input.Skip(StartOffset);

                while (input.Position < input.Length)
                {
                    var subtitle = ReadSubtitle(input);
                    subtitle.PropertyChanged += SubtitlePropertyChanged;
                    result.Add(subtitle);
                }
            }

            LoadChanges(result);
            
            return result;
        }

        public override bool Search(string searchString, string path = "")
        {
            if (string.IsNullOrEmpty(path))
            {
                path = Path;
            }
            var bytes = File.ReadAllBytes(path);

            var pattern = FileEncoding.GetBytes(searchString);

            var searchHelper = new SearchHelper(pattern);
            var index1 = searchHelper.Search(bytes);

            var index2 = -1;
            if (HasChanges)
            {
                bytes = File.ReadAllBytes(ChangesFile);
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
                var dictionary = new Dictionary<long, Subtitle>(subtitles.Count);
                foreach (Subtitle subtitle in subtitles)
                {
                    if (!dictionary.ContainsKey(subtitle.Offset))
                    {
                        dictionary.Add(subtitle.Offset, subtitle);
                    }
                }

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

                        if (dictionary.TryGetValue(offset, out Subtitle subtitle))
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
                if (StartOffset > 0)
                {
                    output.Write(input.ReadBytes(StartOffset));
                }

                long outputOffset = StartOffset;

                while (input.Position < input.Length)
                {
                    var inputSubtitle = ReadSubtitle(input);
                    outputOffset = WriteSubtitle(output, subtitles, inputSubtitle.Offset, outputOffset);
                }
            }
        }

        public override bool SearchText(string searchString, int direction)
        {
            if (_subtitles == null || _subtitles.Count == 0 || _view == null)
            {
                return false;
            }

            List<Subtitle> searchableSubs = _subtitles.Where(x => !string.IsNullOrEmpty(x.Text)).ToList();

            int i;
            int rowIndex;
            if (direction == 0)
            {
                i = 1;
                rowIndex = 1;
            }
            else
            {
                (int item1, Subtitle item2) = _view.GetSelectedSubtitle();
                i = searchableSubs.IndexOf(item2) + direction;
                rowIndex = item1 + direction;
            }

            int step = direction < 0 ? -1 : 1;

            int result = -1;
            while (i >= 0 && i < searchableSubs.Count)
            {
                Subtitle subtitle = searchableSubs[i];
                string original = subtitle.Text;
                string translation = subtitle.Translation;

                if (original.Contains(searchString) || (!string.IsNullOrEmpty(translation) && translation.Contains(searchString)))
                {
                    result = rowIndex;
                    break;
                }

                rowIndex += step;

                i += step;
            }

            bool found = result >= 0;
            if (found)
            {
                _view.DisplaySubtitle(result);
            }

            return found;
        }

        protected virtual void SubtitlePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            NeedSaving = _subtitles.Any(subtitle => subtitle.HasChanges);
            OnFileChanged();
        }

        protected virtual Subtitle ReadSubtitle(ExtendedBinaryReader input)
        {
            var offset = input.Position;
            var subtitle = ReadSubtitle(input, offset, false);
            return subtitle;
        }

        protected virtual Subtitle ReadSubtitle(ExtendedBinaryReader input, long offset, bool returnToPos)
        {
            var pos = input.Position;

            input.Seek(offset, SeekOrigin.Begin);

            var text = input.ReadString();

            var subtitle = new Subtitle
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

        public override void ExportPo(string path)
        {
            var directory = System.IO.Path.GetDirectoryName(path);
            Directory.CreateDirectory(directory);
            
            var po = new Po()
            {
                Header = new PoHeader(GameName, "dummy@dummy.com", "es-ES")
            };

            var subtitles = GetSubtitles();
            foreach (var subtitle in subtitles)
            {
                var entry = new PoEntry();

                var original = subtitle.Text;
                var translation = subtitle.Translation;
                if (string.IsNullOrEmpty(original))
                {
                    original = "<!empty>";
                    translation = "<!empty>";
                }

                entry.Original = original.Replace(LineEnding.ShownLineEnding, LineEnding.PoLineEnding);
                entry.Context = GetContext(subtitle);

                if (original != translation)
                {
                    entry.Translated = translation.Replace(LineEnding.ShownLineEnding, LineEnding.PoLineEnding);
                }

                po.Add(entry);
            }

            var po2binary = new Yarhl.Media.Text.Po2Binary();
            var binary = po2binary.Convert(po);
            
            binary.Stream.WriteTo(path);
        }

        public override void ExportExcel(string path)
        {
            var directory = System.IO.Path.GetDirectoryName(path);
            Directory.CreateDirectory(directory);

            using (var excel = new ExcelPackage())
            {
                var sheet = excel.Workbook.Worksheets.Add("Hoja 1");

                var header = new List<string[]>
                {
                    new[] {"OFFSET", "ORIGINAL", "TRADUCCIÓN" }
                };

                sheet.Cells["A1:C1"].LoadFromArrays(header);

                var row = 2;
                
                var subtitles = GetSubtitles();
                foreach (var subtitle in subtitles)
                {
                    if (string.IsNullOrEmpty(subtitle.Text)) continue;

                    sheet.Cells[row, 1].Value = subtitle.Offset;
                    sheet.Cells[row, 2].Value = subtitle.Text;
                    sheet.Cells[row, 3].Value = subtitle.Translation;

                    row++;
                }

                var excelFile = new FileInfo(path);
                excel.SaveAs(excelFile);
            }
        }

        public override void ImportPo(string inputFile, bool save = true, bool parallel = true)
        {
            try 
            { 
                using (DataStream dataStream = DataStreamFactory.FromFile(inputFile, FileOpenMode.Read))
                {
                    var binary = new BinaryFormat(dataStream);
                    var binary2Po = new Yarhl.Media.Text.Binary2Po();
                    Po po = binary2Po.Convert(binary);

                    LoadBeforeImport();
                    var dictionary = new ConcurrentDictionary<string, Subtitle>();
                    foreach (Subtitle subtitle in _subtitles)
                    {
                        dictionary[GetContext(subtitle)] = subtitle;
                    }

                    void UpdateSubtitleFromPoEntry(PoEntry entry)
                    {
                        string context = entry.Context;
                        if (!dictionary.TryGetValue(context, out Subtitle subtitle))
                        {
                            return;
                        }

                        if (entry.Original == "<!empty>" || string.IsNullOrEmpty(subtitle.Text))
                        {
                            subtitle.Translation = subtitle.Text;
                        }
                        else
                        {
                            if (entry.Original.Replace(LineEnding.PoLineEnding, LineEnding.ShownLineEnding) !=
                                subtitle.Text)
                            {
                                // El texto original de la entrada no coincide con el del subtítulo, así que no podemos aplicar la traducción
                                subtitle.Translation = subtitle.Text;
                            }
                            else
                            {
                                string translation = entry.Translated;
                                subtitle.Translation = string.IsNullOrEmpty(translation)
                                    ? subtitle.Text
                                    : translation.Replace(LineEnding.PoLineEnding, LineEnding.ShownLineEnding);
                            }
                        }
                    }

                    if (parallel)
                    {
                        Parallel.ForEach(po.Entries, UpdateSubtitleFromPoEntry);
                    }
                    else
                    {
                        foreach (PoEntry entry in po.Entries)
                        {
                            UpdateSubtitleFromPoEntry(entry);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                // Ignora o erro
            }

            if (save && NeedSaving)
            {
                SaveChanges();
            }
        }

        public override void ImportExcel(string inputFile, BackgroundWorker worker, int porcentagem, bool save = true)
        {
            if (!GameName.Contains("Yakuza"))
            {
                worker.ReportProgress(porcentagem, "O importador automático não está programado para esse jogo!");
                return;
            }

            var strings = new Dictionary<string, string>();

            try
            {
                using (var stream = File.Open(inputFile, FileMode.Open, FileAccess.Read))
                {
                    LoadBeforeImport();

                    // Auto-detect format, supports:
                    //  - Binary Excel files (2.0-2003 format; *.xls)
                    //  - OpenXml Excel files (2007 format; *.xlsx)
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        var content = reader.AsDataSet();

                        var table = content.Tables[0];

                        for (var i = 0; i < table.Rows.Count; i++)
                        {
                            var key = table.Rows[i][0].ToString();
                            var value = table.Rows[i][2].ToString();

                            if (!string.IsNullOrEmpty(key) && !strings.ContainsKey(key))
                            {
                                strings.Add(key, value);
                            }
                        }
                    }
                }

                foreach (var subtitle in _subtitles)
                {
                    var key = subtitle.Offset.ToString();

                    if (string.IsNullOrEmpty(key) || !strings.ContainsKey(key)) continue;

                    var values = strings[key].Split('\t');
                    subtitle.Translation = values[0];
                }
            }
            catch (Exception)
            {
                worker.ReportProgress(porcentagem, $"Erro ao processar o arquivo: {inputFile}");
            }

            if (save && NeedSaving)
            {
                SaveChanges();
            }
        }

        protected override void LoadBeforeImport()
        {
            if (_subtitles == null)
            {
                _subtitles = GetSubtitles();
            }
        }

        protected override string GetContext(Subtitle subtitle)
        {
            return subtitle.Offset.ToString();
        }
    }
}
