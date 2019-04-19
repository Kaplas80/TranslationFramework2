using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DataGridViewNumericUpDownElements;
using ExcelDataReader;
using OfficeOpenXml;
using TF.Core.TranslationEntities;
using WeifenLuo.WinFormsUI.Docking;

namespace TFGame.TheMissing.Files.Txt
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

        public event AutoAdjustEventHandler AutoAdjustSizes;
        public delegate void AutoAdjustEventHandler();

        protected IList<Subtitle> _subtitles;

        protected GridView()
        {
            InitializeComponent();

            var cellStyle = new DataGridViewCellStyle();
            cellStyle.Font = TF.Core.Fonts.FontCollection.GetFont("Noto Sans CJK JP Regular", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            SubtitleGridView.RowsDefaultCellStyle = cellStyle;
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
                ReadOnly = true
            };
            SubtitleGridView.Columns.Add(column);

            column = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Text",
                Name = "colOriginal",
                HeaderText = "Original",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = new DataGridViewCellStyle {BackColor = Color.LightGray},
                ReadOnly = true
            };
            SubtitleGridView.Columns.Add(column);

            column = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Translation",
                Name = "colTranslation",
                HeaderText = "Traducción",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            };
            SubtitleGridView.Columns.Add(column);

            var nudColumn = new DataGridViewNumericUpDownColumn
            {
                DataPropertyName = "Width",
                Name = "colWidth",
                HeaderText = "Ancho",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DecimalPlaces = 0,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight },
                Maximum = new decimal(1000),
                Minimum = new decimal(-1),
                Increment = new decimal(10),
                FillWeight = 25F,
            };
            SubtitleGridView.Columns.Add(nudColumn);

            nudColumn = new DataGridViewNumericUpDownColumn
            {
                DataPropertyName = "Height",
                Name = "colHeight",
                HeaderText = "Alto",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DecimalPlaces = 0,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight },
                Maximum = new decimal(1000),
                Minimum = new decimal(-1),
                Increment = new decimal(24),
                FillWeight = 25F,
            };
            SubtitleGridView.Columns.Add(nudColumn);

            UpdateLabel();
        }

        public void DisplaySubtitle(int index)
        {
            if (index == -1)
            {
                return;
            }

            SubtitleGridView.ClearSelection();
            SubtitleGridView.Rows[index].Cells["colTranslation"].Selected = true;
            SubtitleGridView.FirstDisplayedScrollingRowIndex = index;
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

            var subtitle = _subtitles[e.RowIndex];

            if (e.ColumnIndex == 2)
            {
                if (!e.State.HasFlag(DataGridViewElementStates.Selected) && (subtitle.Text != subtitle.Translation) &&
                    (!string.IsNullOrEmpty(subtitle.Translation)))
                {
                    e.CellStyle.BackColor = Color.AntiqueWhite;
                }
            }

            if (e.ColumnIndex == 3 || e.ColumnIndex == 4)
            {
                if (subtitle.Width < 0)
                {
                    e.CellStyle.BackColor = Color.LightGray;
                }
            }

            if (e.ColumnIndex == 3)
            {
                if (!e.State.HasFlag(DataGridViewElementStates.Selected) && (subtitle.LoadedWidth != subtitle.Width))
                {
                    e.CellStyle.BackColor = Color.AntiqueWhite;
                }
            }

            if (e.ColumnIndex == 4)
            {
                if (!e.State.HasFlag(DataGridViewElementStates.Selected) && (subtitle.LoadedHeight != subtitle.Height))
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

        private void SubtitleGridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            SubtitleGridView.BeginEdit(false);
        }

        private void SubtitleGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control.GetType() == typeof(DataGridViewTextBoxEditingControl))
            {
                SendKeys.Send("{RIGHT}");
            }
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
                    new[] {"OFFSET", "ORIGINAL", "TRADUCCIÓN", "ANCHO", "ALTO" }
                };

                sheet.Cells["A1:E1"].LoadFromArrays(header);

                var row = 2;
                foreach (var subtitle in _subtitles)
                {
                    sheet.Cells[row, 1].Value = subtitle.Offset;
                    sheet.Cells[row, 2].Value = subtitle.Text;
                    sheet.Cells[row, 3].Value = subtitle.Translation;
                    sheet.Cells[row, 4].Value = subtitle.Width;
                    sheet.Cells[row, 5].Value = subtitle.Height;

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
                using (var stream = System.IO.File.Open(ImportFileDialog.FileName, FileMode.Open, FileAccess.Read))
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
                            var key = useOffset ? table.Rows[i][0].ToString() : table.Rows[i][1].ToString();

                            var value = string.Concat(table.Rows[i][2].ToString(), "\t", table.Rows[i][3].ToString(), "\t", table.Rows[i][4].ToString());

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
                        var values = strings[key].Split('\t');
                        subtitle.Translation = values[0];
                        subtitle.Width = float.Parse(values[1]);
                        subtitle.Height = float.Parse(values[2]);
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

        private void SubtitleGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            UpdateLabel();
        }

        private void UpdateLabel()
        {
            var changedLines = _subtitles.Count(x => x.Text != x.Translation);
            var totalLines = _subtitles.Count;
            lblChangedLinesCount.Text = $"Líneas modificadas: {changedLines}/{totalLines}";
        }

        private void SubtitleGridView_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            var subtitle = _subtitles[e.RowIndex];

            if (subtitle.Width < 0 && subtitle.Height < 0)
            {
                if (e.ColumnIndex == 3 || e.ColumnIndex == 4)
                {
                    e.Cancel = true;
                }
            }
        }

        private void btnAutoAdjust_Click(object sender, EventArgs e)
        {
            AutoAdjustSizes?.Invoke();
        }
    }
}