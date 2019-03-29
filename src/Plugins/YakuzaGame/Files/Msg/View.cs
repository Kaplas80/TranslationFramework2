using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Be.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace YakuzaGame.Files.Msg
{
    public partial class View : DockContent
    {
        // Con esta clase evito que la flecha izquierda cambie de celda al editar
        private class TFDataGridView : DataGridView
        {
            [SecurityPermission(
                SecurityAction.LinkDemand, Flags =
                    SecurityPermissionFlag.UnmanagedCode)]
            protected override bool ProcessDataGridViewKey(KeyEventArgs e)
            {
                if (e.KeyCode == Keys.Left)
                {
                    return true;
                }
                return base.ProcessDataGridViewKey(e);
            }
        }

        private IList<Subtitle> _subtitles;

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
                DefaultCellStyle = new DataGridViewCellStyle { Format = "X8", BackColor = Color.LightGray},
                ReadOnly = true
            };
            SubtitleGridView.Columns.Add(column);

            column = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Text",
                Name = "colOriginal",
                HeaderText = "Original",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.LightGray },
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

            if (e.ColumnIndex == 0)
            {
                var subtitle = _subtitles[e.RowIndex];

                if (subtitle.Properties != null)
                {
                    if (!subtitle.Properties.ToByteArray().SequenceEqual(subtitle.TranslationProperties.ToByteArray()))
                    {
                        e.CellStyle.BackColor = Color.Red;
                    }
                    else
                    {
                        e.CellStyle.BackColor = SubtitleGridView.Columns[0].DefaultCellStyle.BackColor;
                    }
                }
                else
                {
                    e.CellStyle.BackColor = SubtitleGridView.Columns[0].DefaultCellStyle.BackColor;
                }
            }

            if (e.ColumnIndex == 2)
            {
                var subtitle = _subtitles[e.RowIndex];

                if (!e.State.HasFlag(DataGridViewElementStates.Selected) && (subtitle.Text != subtitle.Translation) && (!string.IsNullOrEmpty(subtitle.Translation)))
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
            SubtitleGridView.Rows[index].Cells["colTranslation"].Selected = true;
            SubtitleGridView.FirstDisplayedScrollingRowIndex = index;
        }

        public Tuple<int,Subtitle> GetSelectedSubtitle()
        {
            var rowIndex = SubtitleGridView.SelectedCells[0].RowIndex;
            var subtitles = (IList<Subtitle>) SubtitleGridView.DataSource;
            return new Tuple<int, Subtitle>(rowIndex, subtitles[rowIndex]);
        }

        private void SubtitleGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (SubtitleGridView.SelectedCells.Count == 1)
            {
                var subtitle =
                    ((List<Subtitle>) SubtitleGridView.DataSource)[SubtitleGridView.SelectedCells[0].RowIndex];

                if (subtitle.Properties != null)
                {
                    var byteProvider = new DynamicByteProvider(subtitle.Properties.ToByteArray());
                    hexBox1.ByteProvider = byteProvider;

                    var byteProvider2 = new DynamicByteProvider(subtitle.TranslationProperties.ToByteArray());
                    hexBox2.ByteProvider = byteProvider2;
                }
                else
                {
                    hexBox1.ByteProvider = null;
                    hexBox2.ByteProvider = null;
                }
            }
            else
            {
                hexBox1.ByteProvider = null;
                hexBox2.ByteProvider = null;
            }
        }
    }
}
