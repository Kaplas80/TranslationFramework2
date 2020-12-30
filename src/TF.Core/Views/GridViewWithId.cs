namespace TF.Core.Views
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;
    using ExcelDataReader;
    using OfficeOpenXml;
    using TF.Core.Entities;
    using TF.Core.TranslationEntities;

    public class GridViewWithId : GridView
    {
        public GridViewWithId(TranslationFile file) : base(file)
        {
        }

        public override void LoadData(IList<Subtitle> subtitles)
        {
            _subtitles = subtitles;

            SubtitleGridView.AutoGenerateColumns = false;

            var subs = new List<SubtitleWithId>(_subtitles.Count);
            subs.AddRange(_subtitles.Select(subtitle => subtitle as SubtitleWithId));

            SubtitleGridView.DataSource = subs;

            var column = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Id",
                Name = "colId",
                HeaderText = "Id",
                DefaultCellStyle = new DataGridViewCellStyle {BackColor = Color.LightGray},
                ReadOnly = true,
                SortMode = DataGridViewColumnSortMode.NotSortable,
            };
            SubtitleGridView.Columns.Add(column);

            column = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Text",
                Name = "colOriginal",
                HeaderText = "Original",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = new DataGridViewCellStyle {BackColor = Color.LightGray},
                ReadOnly = true,
                SortMode = DataGridViewColumnSortMode.NotSortable,
            };
            SubtitleGridView.Columns.Add(column);

            column = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Translation",
                Name = "colTranslation",
                HeaderText = "Traducción",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                ReadOnly = true,
                SortMode = DataGridViewColumnSortMode.NotSortable,
            };
            SubtitleGridView.Columns.Add(column);

            UpdateLabel();

            _selectedSubtitle = null;
            _selectedSubtitleIndex = -1;
        }

        protected override void btnExport_Click(object sender, EventArgs e)
        {
            ExportFileDialog.Filter = "Archivos Excel|*.xlsx";
            ExportFileDialog.FileName = string.Concat(Path.GetFileNameWithoutExtension(_file.Path), ".xlsx");
            var result = ExportFileDialog.ShowDialog(this);

            if (result != DialogResult.OK)
            {
                return;
            }

            using (var excel = new ExcelPackage())
            {
                var sheet = excel.Workbook.Worksheets.Add("Hoja 1");

                var header = new List<string[]>
                {
                    new[] {"ID", "ORIGINAL", "TRADUCCIÓN"}
                };

                sheet.Cells["A1:C1"].LoadFromArrays(header);

                var row = 2;
                foreach (var subtitle in _subtitles)
                {
                    var subtitleWithId = subtitle as SubtitleWithId;
                    sheet.Cells[row, 1].Value = subtitleWithId.Id;
                    sheet.Cells[row, 2].Value = subtitleWithId.Text;
                    sheet.Cells[row, 3].Value = subtitleWithId.Translation;

                    row++;
                }

                var excelFile = new FileInfo(ExportFileDialog.FileName);
                excel.SaveAs(excelFile);
            }
        }

        protected override void Import(bool useOffset)
        {
            ImportFileDialog.Filter = "Archivos Excel|*.xlsx";
            ImportFileDialog.FileName = string.Concat(Path.GetFileNameWithoutExtension(_file.Path), ".xlsx");
            var result = ImportFileDialog.ShowDialog(this);

            if (result != DialogResult.OK)
            {
                return;
            }

            var strings = new Dictionary<string, Tuple<string, string>>();
            try
            {
                using (var stream = File.Open(ImportFileDialog.FileName, FileMode.Open, FileAccess.Read))
                {
                    // Auto-detect format, supports:
                    //  - Binary Excel files (2.0-2003 format; *.xls)
                    //  - OpenXml Excel files (2007 format; *.xlsx)
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {

                        var content = reader.AsDataSet();

                        var table = content.Tables[0];

                        for (var i = 0; i < table.Rows.Count; i++)
                        {
                            string key;
                            string original;
                            string trad;
                            if (useOffset)
                            {
                                key = table.Rows[i][0].ToString();
                                original = table.Rows[i][1].ToString();
                                trad = table.Rows[i][2].ToString();
                            }
                            else
                            {
                                key = table.Rows[i][1].ToString();
                                original = table.Rows[i][1].ToString();
                                trad = table.Rows[i][2].ToString();
                            }

                            if (!string.IsNullOrEmpty(key) && !strings.ContainsKey(key))
                            {
                                strings.Add(key, new Tuple<string, string>(original, trad));
                            }
                        }
                    }
                }

                foreach (var subtitle in _subtitles)
                {
                    var subtitleWithId = subtitle as SubtitleWithId;
                    var key = useOffset ? subtitleWithId.Id.ToString() : subtitleWithId.Text;

                    if (!string.IsNullOrEmpty(key) && strings.ContainsKey(key))
                    {
                        var value = strings[key];
                        if (subtitleWithId.Text == value.Item1)
                        {
                            subtitleWithId.Translation = value.Item2;
                        }
                    }
                }

                _selectedSubtitle = null;
                SubtitleGridView.Invalidate();
                DisplaySubtitle(_selectedSubtitleIndex);
                UpdateLabel();
            }
            catch (Exception e)
            {
                MessageBox.Show($"No se ha podido abrir el fichero.\r\n{e.GetType()}: {e.Message}", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
