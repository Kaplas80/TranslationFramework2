using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ExcelDataReader;
using OfficeOpenXml;
using ScintillaNET;
using TF.Core.Helpers;
using TF.Core.TranslationEntities;
using WeifenLuo.WinFormsUI.Docking;

namespace TF.Core.Views
{
    public partial class GridView : DockContent
    {
        // Con esta clase evito que la flechas izquierda y derecha cambien de celda al editar
        private class TFDataGridView : DataGridView
        {
            [SecurityPermission(
                SecurityAction.LinkDemand, Flags =
                    SecurityPermissionFlag.UnmanagedCode)]
            protected override bool ProcessDataGridViewKey(KeyEventArgs e)
            {
                if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
                {
                    return true;
                }

                return base.ProcessDataGridViewKey(e);
            }
        }

        protected IList<Subtitle> _subtitles;
        protected Subtitle _selectedSubtitle;
        protected int _selectedSubtitleIndex;

        protected GridView()
        {
            InitializeComponent();

            var font = Fonts.FontCollection.GetFont("Noto Sans CJK JP Regular", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            var cellStyle = new DataGridViewCellStyle();
            cellStyle.Font = font;
            SubtitleGridView.RowsDefaultCellStyle = cellStyle;

            InitScintilla(scintilla1);
            InitScintilla(scintilla2);
        }

        private static void InitScintilla(Scintilla scintilla)
        {
            scintilla.StyleResetDefault();
            scintilla.Styles[Style.Default].Font = "Noto Sans CJK JP Regular";
            scintilla.Styles[Style.Default].Size = 11;
            scintilla.StyleClearAll();

            scintilla.Styles[Style.Xml.Tag].ForeColor = Color.Blue;
            scintilla.Styles[Style.Xml.TagEnd].ForeColor = Color.Blue;
            scintilla.Lexer = Lexer.Xml;
        }

        public GridView(ThemeBase theme) : this()
        {
            dockPanel1.Theme = theme;
            dockPanel1.DocumentStyle = DocumentStyle.DockingSdi;
        }

        public void LoadData(IList<Subtitle> subtitles)
        {
            _subtitles = subtitles;

            SubtitleGridView.AutoGenerateColumns = false;
            SubtitleGridView.DataSource = _subtitles;

            var column = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Offset",
                Name = "colOffset",
                HeaderText = "Offset",
                DefaultCellStyle = new DataGridViewCellStyle {Format = "X8", BackColor = Color.LightGray},
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

        public void DisplaySubtitle(int index)
        {
            if (index == -1)
            {
                return;
            }

            SubtitleGridView.ClearSelection();
            SubtitleGridView.Rows[index].Cells["colTranslation"].Selected = true;
        }

        public Tuple<int, Subtitle> GetSelectedSubtitle()
        {
            var rowIndex = SubtitleGridView.SelectedCells[0].RowIndex;
            var subtitles = (IList<Subtitle>) SubtitleGridView.DataSource;
            return new Tuple<int, Subtitle>(rowIndex, subtitles[rowIndex]);
        }

        protected virtual void SubtitleGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0)
            {
                return;
            }

            if (e.Value == null)
            {
                return;
            }

            if (e.ColumnIndex == 2)
            {
                var subtitle = _subtitles[e.RowIndex];

                if (!e.State.HasFlag(DataGridViewElementStates.Selected) && (subtitle.Text != subtitle.Translation) &&
                    (!string.IsNullOrEmpty(subtitle.Translation)))
                {
                    e.CellStyle.BackColor = Color.AntiqueWhite;
                }
            }

            var lineBreakPattern = @"(\\r\\n)|(\\n)";
            var tagPattern = @"(<[^>]*>|%s|%[\d]*d|%u|%[[\d]+[\.\d]*]*f)";
            var strSplit = Regex.Split(e.Value.ToString(), $@"{lineBreakPattern}|{tagPattern}");

            if (strSplit.Length <= 1)
            {
                return;
            }

            var defaultColor = e.State.HasFlag(DataGridViewElementStates.Selected)
                ? e.CellStyle.SelectionForeColor
                : e.CellStyle.ForeColor;
            var lineBreakColor = Color.Red;
            var tagColor = e.State.HasFlag(DataGridViewElementStates.Selected) ? Color.Azure : Color.Blue;

            var rect = new Rectangle(e.CellBounds.X + 3, e.CellBounds.Y - 1, e.CellBounds.Width - 6,
                e.CellBounds.Height);
            var x = rect.X;

            var proposedSize = new Size(int.MaxValue, int.MaxValue);
            const TextFormatFlags formatFlags =
                TextFormatFlags.NoPrefix | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding;

            e.Paint(e.CellBounds,
                DataGridViewPaintParts.Background | DataGridViewPaintParts.Border | DataGridViewPaintParts.Focus |
                DataGridViewPaintParts.SelectionBackground);

            foreach (var s in strSplit)
            {
                if (string.IsNullOrEmpty(s))
                {
                    continue;
                }

                Color c;
                if (Regex.IsMatch(s, lineBreakPattern))
                {
                    c = lineBreakColor;
                }
                else if (Regex.IsMatch(s, tagPattern))
                {
                    c = tagColor;
                }
                else
                {
                    c = defaultColor;
                }

                TextRenderer.DrawText(e.Graphics, s,
                    e.CellStyle.Font, rect, c, formatFlags);

                x += TextRenderer.MeasureText(e.Graphics, s, e.CellStyle.Font, proposedSize, formatFlags).Width;

                rect.Width = rect.Width - (x - rect.X);
                rect.X = x;
            }

            e.Handled = true;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
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
                    new[] {"OFFSET", "ORIGINAL", "TRADUCCIÓN"}
                };

                sheet.Cells["A1:C1"].LoadFromArrays(header);

                var row = 2;
                foreach (var subtitle in _subtitles)
                {
                    sheet.Cells[row, 1].Value = subtitle.Offset;
                    sheet.Cells[row, 2].Value = subtitle.Text;
                    sheet.Cells[row, 3].Value = subtitle.Translation;

                    row++;
                }

                var excelFile = new FileInfo(ExportFileDialog.FileName);
                excel.SaveAs(excelFile);
            }
        }

        private void btnOffsetImport_Click(object sender, EventArgs e)
        {
            Import(true);
        }

        private void btnSimpleImport_Click(object sender, EventArgs e)
        {
            Import(false);
        }

        private void Import(bool useOffset)
        {
            var result = ImportFileDialog.ShowDialog(this);

            if (result != DialogResult.OK)
            {
                return;
            }

            var strings = new Dictionary<string, string>();
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
                            string value;
                            if (useOffset)
                            {
                                key = table.Rows[i][0].ToString();
                                value = table.Rows[i][2].ToString();
                            }
                            else
                            {
                                key = table.Rows[i][1].ToString();
                                value = table.Rows[i][2].ToString();
                            }

                            if (!string.IsNullOrEmpty(key) && !strings.ContainsKey(key))
                            {
                                strings.Add(key, value);
                            }
                        }
                    }
                }

                foreach (var subtitle in _subtitles)
                {
                    var key = useOffset ? subtitle.Offset.ToString() : subtitle.Text;

                    if (!string.IsNullOrEmpty(key) && strings.ContainsKey(key))
                    {
                        subtitle.Translation = strings[key];
                    }
                }

                SubtitleGridView.Invalidate();
                UpdateLabel();
            }
            catch (Exception e)
            {
                MessageBox.Show($"No se ha podido abrir el fichero.\r\n{e.GetType()}: {e.Message}", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateLabel()
        {
            var changedLines = _subtitles.Count(x => x.Text != x.Translation);
            var totalLines = _subtitles.Count;
            lblChangedLinesCount.Text = $"Líneas modificadas: {changedLines}/{totalLines}";
        }

        private void SubtitleGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (_selectedSubtitle != null)
            {
                if (scintilla2.Modified)
                {
                    _selectedSubtitle.Translation = scintilla2.Text.Replace("\n", "\\n");
                }
            }

            if (SubtitleGridView.SelectedCells.Count != 0)
            {
                var (index, subtitle) = GetSelectedSubtitle();
                _selectedSubtitleIndex = index;
                _selectedSubtitle = subtitle;

                scintilla1.ReadOnly = false;
                scintilla1.Text = _selectedSubtitle.Text.Replace("\\n", "\n");
                scintilla1.ReadOnly = true;
                scintilla2.Text = _selectedSubtitle.Translation.Replace("\\n", "\n");
            }
        }

        private void Scintilla2_TextChanged(object sender, EventArgs e)
        {
            if (_selectedSubtitle != null)
            {
                if (scintilla2.Modified)
                {
                    _selectedSubtitle.Translation = scintilla2.Text.Replace("\n", "\\n");
                    SubtitleGridView.Invalidate();
                    UpdateLabel();
                }
            }
        }

        private void RestaurarTextoOriginalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_selectedSubtitle != null)
            {
                scintilla2.Text = scintilla1.Text;
            }
        }

        private void DeshacerCambiosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_selectedSubtitle != null)
            {
                scintilla2.Text = _selectedSubtitle.Loaded.Replace("\\n", "\n");
            }
        }

        private void ContextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            preTraducirToolStripMenuItem.Enabled = File.Exists("MicrosoftTranslator.txt");
        }

        private void PreTraducirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_selectedSubtitle != null)
            {
                scintilla2.Text = AutomaticTranslationHelper.Translate(scintilla1.Text);
            }
        }

        private void Scintilla2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.Return)
            {
                if (_selectedSubtitleIndex < SubtitleGridView.RowCount)
                {
                    e.SuppressKeyPress = true;
                    DisplaySubtitle(_selectedSubtitleIndex+1);
                }
            }
        }
    }
}