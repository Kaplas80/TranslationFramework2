using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Security.Permissions;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ExcelDataReader;
using OfficeOpenXml;
using WeifenLuo.WinFormsUI.Docking;

namespace YakuzaGame.Files.Table
{
    public partial class View : DockContent
    {
        // Con esta clase evito que la flecha izquierda o derecha cambien de celda al editar
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

        private TableData _data;

        protected View()
        {
            InitializeComponent();

            var cellStyle = new DataGridViewCellStyle();
            cellStyle.Font = TF.Core.Fonts.FontCollection.GetFont("Noto Sans CJK JP Regular", 9.75F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            cellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            cellStyle.BackColor = SystemColors.Window;
            cellStyle.ForeColor = SystemColors.ControlText;
            cellStyle.SelectionBackColor = SystemColors.Highlight;
            cellStyle.SelectionForeColor = SystemColors.HighlightText;
            cellStyle.WrapMode = DataGridViewTriState.False;

            SubtitleGridView.RowsDefaultCellStyle = cellStyle;
        }

        public View(ThemeBase theme) : this()
        {
            dockPanel1.Theme = theme;

            dockPanel1.DocumentStyle = DocumentStyle.DockingSdi;
        }

        public void LoadData(TableData data)
        {
            _data = data;

            SubtitleGridView.AutoGenerateColumns = false;

            foreach (var dataColumn in data.Columns)
            {
                if (dataColumn.GetType().Name != nameof(Column) && dataColumn.DataCount > 0 && dataColumn.Size > 0)
                {
                    var column = new DataGridViewTextBoxColumn
                    {
                        Name = dataColumn.Name,
                        HeaderText = $"{dataColumn.Name}",
                        ReadOnly = true,
                        DefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.LightGray },
                        Tag = dataColumn
                    };

                    SubtitleGridView.Columns.Add(column);

                    column = new DataGridViewTextBoxColumn
                    {
                        Name = $"{dataColumn.Name}_TRAD",
                        HeaderText = $"{dataColumn.Name} (TRAD)",
                        ReadOnly = false,
                        Tag = dataColumn
                    };

                    SubtitleGridView.Columns.Add(column);
                }
            }

            SubtitleGridView.RowCount = _data.NumRows;
        }

        private void SubtitleGridView_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            var isTranslationColumn = (e.ColumnIndex % 2) == 1;
            var column = (Column)SubtitleGridView.Columns[e.ColumnIndex].Tag;

            if (column != null)
            {
                e.Value = isTranslationColumn ? column.GetTranslatedValue(e.RowIndex) : column.GetValue(e.RowIndex);
            }
        }

        private void SubtitleGridView_CellValuePushed(object sender, DataGridViewCellValueEventArgs e)
        {
            var isTranslationColumn = (e.ColumnIndex % 2) == 1;
            var column = (Column)SubtitleGridView.Columns[e.ColumnIndex].Tag;

            if (isTranslationColumn)
            {
                column.SetTranslatedValue(e.RowIndex, e.Value.ToString());
                SubtitleGridView.InvalidateColumn(e.ColumnIndex);
            }
        }

        private void SubtitleGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0)
            {
                return;
            }

            if (e.Value == null)
            {
                return;
            }

            if (e.ColumnIndex % 2 == 1)
            {
                var column = (Column)SubtitleGridView.Columns[e.ColumnIndex].Tag;

                var text = column.GetValue(e.RowIndex);
                var translatedText = column.GetTranslatedValue(e.RowIndex);
                if (!e.State.HasFlag(DataGridViewElementStates.Selected) && (text != translatedText) && (!string.IsNullOrEmpty(translatedText)))
                {
                    e.CellStyle.BackColor = Color.AntiqueWhite;
                }
            }
            else
            {
                e.CellStyle.BackColor = Color.LightGray;
            }

            var lineBreakPattern = @"(\\r\\n)|(\\n)";
            var tagPattern = @"(<[^>]*>|%s|%[\d]*d|%u|%[[\d]+[\.\d]*]*f)";
            var strSplit = Regex.Split(e.Value.ToString(), $@"{lineBreakPattern}|{tagPattern}");

            if (strSplit.Length <= 1)
            {
                return;
            }

            var defaultColor = e.State.HasFlag(DataGridViewElementStates.Selected) ? e.CellStyle.SelectionForeColor : e.CellStyle.ForeColor;
            var lineBreakColor = Color.Red;
            var tagColor = e.State.HasFlag(DataGridViewElementStates.Selected) ? Color.Azure : Color.Blue;

            var rect = new Rectangle(e.CellBounds.X + 3, e.CellBounds.Y - 1, e.CellBounds.Width - 6, e.CellBounds.Height);
            var x = rect.X;

            var proposedSize = new Size(int.MaxValue, int.MaxValue);
            const TextFormatFlags formatFlags = TextFormatFlags.NoPrefix | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding;

            e.Paint(e.CellBounds, DataGridViewPaintParts.Background | DataGridViewPaintParts.Border | DataGridViewPaintParts.Focus | DataGridViewPaintParts.SelectionBackground);

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

        public void DisplaySubtitle(int index)
        {
            if (index == -1)
            {
                return;
            }

            SubtitleGridView.ClearSelection();
            SubtitleGridView.Rows[index].Cells[0].Selected = true;
            SubtitleGridView.FirstDisplayedScrollingRowIndex = index;
        }

        public int GetSelectedRow()
        {
            var rowIndex = SubtitleGridView.SelectedCells[0].RowIndex;
            return rowIndex;
        }

        private void btnExport_Click(object sender, System.EventArgs e)
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
                    new[] {"COLUMNA", "ORDEN", "ORIGINAL", "TRADUCCIÓN"}
                };

                sheet.Cells["A1:D1"].LoadFromArrays(header);

                var row = 2;
                foreach (var column in _data.Columns)
                {
                    var uniqueValues = column.GetUniqueValues();

                    var i = 0;
                    foreach (var (original, translation) in uniqueValues)
                    {
                        sheet.Cells[row, 1].Value = column.Name;
                        sheet.Cells[row, 2].Value = i;
                        sheet.Cells[row, 3].Value = original;
                        sheet.Cells[row, 4].Value = translation;

                        i++;
                        row++;
                    }
                }

                var excelFile = new FileInfo(ExportFileDialog.FileName);
                excel.SaveAs(excelFile);
            }
        }

        private void btnColumnImport_Click(object sender, System.EventArgs e)
        {
            Import(true);
        }

        private void btnSimpleImport_Click(object sender, System.EventArgs e)
        {
            Import(false);
        }

        private void Import(bool useOrder)
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
                            string key;
                            string value;
                            if (useOrder)
                            {
                                key = string.Concat(table.Rows[i][0].ToString(), "|", table.Rows[i][1].ToString());
                                value = table.Rows[i][3].ToString();
                            }
                            else
                            {
                                key = table.Rows[i][2].ToString();
                                value = table.Rows[i][3].ToString();
                            }

                            if (!string.IsNullOrEmpty(key) && !strings.ContainsKey(key))
                            {
                                strings.Add(key, value);
                            }
                        }
                    }
                }

                foreach (var column in _data.Columns)
                {
                    column.SetUniqueValues(strings, useOrder);
                }

                SubtitleGridView.Invalidate();

            }
            catch (Exception e)
            {
                MessageBox.Show($"No se ha podido abrir el fichero.\r\n{e.GetType()}: {e.Message}", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
