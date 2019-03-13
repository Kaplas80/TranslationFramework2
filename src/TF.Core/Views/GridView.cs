using System;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Permissions;
using System.Text.RegularExpressions;
using System.Windows.Forms;
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

        private IList<Subtitle> _subtitles;

        protected GridView()
        {
            InitializeComponent();
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
            var subtitles = (IList<Subtitle>)SubtitleGridView.DataSource;
            return new Tuple<int, Subtitle>(rowIndex, subtitles[rowIndex]);
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
    }
}