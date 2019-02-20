using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace YakuzaCommon.Files.SimpleSubtitle
{
    public partial class View : DockContent
    {
        private IList<Subtitle> _subtitles;

        protected View()
        {
            InitializeComponent();
        }

        public View(ThemeBase theme) : this()
        {
            dockPanel1.Theme = theme;

            dockPanel1.DocumentStyle = DocumentStyle.DockingSdi;
        }

        internal void LoadSubtitles(IList<Subtitle> subtitles)
        {
            _subtitles = subtitles;

            SubtitleGridView.AutoGenerateColumns = false;
            SubtitleGridView.DataSource = _subtitles;

            var column = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Offset",
                Name = "colOffset",
                HeaderText = "Offset",
                DefaultCellStyle = new DataGridViewCellStyle { Format = "X8" },
                ReadOnly = true
            };
            SubtitleGridView.Columns.Add(column);

            column = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Text",
                Name = "colOriginal",
                HeaderText = "Original",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
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

                if (!e.State.HasFlag(DataGridViewElementStates.Selected) && (subtitle.Text != subtitle.Translation) && (!string.IsNullOrEmpty(subtitle.Translation)))
                {
                    e.CellStyle.BackColor = Color.AntiqueWhite;
                }
            }

            var lbPattern = @"(\\n)";
            var strSplit = Regex.Split(e.Value.ToString(), $@"{lbPattern}");

            if (strSplit.Length <= 1)
            {
                return;
            }

            var defaultColor = e.State.HasFlag(DataGridViewElementStates.Selected) ? e.CellStyle.SelectionForeColor : e.CellStyle.ForeColor;
            var lineBreakColor = Color.Red;

            var rect = new Rectangle(e.CellBounds.X + 3, e.CellBounds.Y - 1, e.CellBounds.Width - 6, e.CellBounds.Height);
            var x = rect.X;

            var proposedSize = new Size(int.MaxValue, int.MaxValue);
            const TextFormatFlags formatFlags = TextFormatFlags.NoPrefix | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding;

            e.Paint(e.CellBounds, DataGridViewPaintParts.Background | DataGridViewPaintParts.Border | DataGridViewPaintParts.Focus | DataGridViewPaintParts.SelectionBackground);

            foreach (var s in strSplit)
            {
                Color c;
                if (Regex.IsMatch(s, lbPattern))
                {
                    c = lineBreakColor;
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
    }
}
