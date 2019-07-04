using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using DataGridViewNumericUpDownElements;
using WeifenLuo.WinFormsUI.Docking;

namespace TFGame.TrailsSky.Files.Exe
{
    public partial class FontTableView : DockContent
    {
        private CharacterInfo[] _data;

        protected FontTableView()
        {
            InitializeComponent();
        }

        public FontTableView(ThemeBase theme) : this()
        {
            dockPanel1.Theme = theme;

            dockPanel1.DocumentStyle = DocumentStyle.DockingSdi;
        }

        internal void LoadFontTable(CharacterInfo[] data)
        {
            _data = data;

            FontCharsGridView.AutoGenerateColumns = false;

            var colId = new DataGridViewTextBoxColumn
            {
                Name = "colId",
                HeaderText = "Id",
                DefaultCellStyle = new DataGridViewCellStyle { Format = "X2", BackColor = Color.LightGray},
                ReadOnly = true,
                SortMode = DataGridViewColumnSortMode.NotSortable,
            };
            FontCharsGridView.Columns.Add(colId);

            var colChr = new DataGridViewTextBoxColumn
            {
                Name = "colChr",
                HeaderText = "Chr",
                DefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.LightGray },
                ReadOnly = true,
                SortMode = DataGridViewColumnSortMode.NotSortable,
            };
            FontCharsGridView.Columns.Add(colChr);

            var column = new DataGridViewNumericUpDownColumn
            {
                Name = "colWidth",
                HeaderText = "Ancho",
                DecimalPlaces = 0,
                DefaultCellStyle = new DataGridViewCellStyle {Alignment = DataGridViewContentAlignment.MiddleRight},
                Increment = new decimal(1),
                Maximum = new decimal(0x10),
                Minimum = new decimal(0),
                SortMode = DataGridViewColumnSortMode.NotSortable,
            };
            FontCharsGridView.Columns.Add(column);

            FontCharsGridView.RowCount = 128;
        }

        private void FontCharsGridView_CellValuePushed(object sender, DataGridViewCellValueEventArgs e)
        {
            var index = e.ColumnIndex - 2;

            if (index >= 0)
            {
                _data[e.RowIndex].Width = Convert.ToInt32(e.Value);
            }
        }

        private void FontCharsGridView_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                e.Value = _data[e.RowIndex].Id;
                return;
            }

            if (e.ColumnIndex == 1)
            {
                e.Value = _data[e.RowIndex].Chr;
                return;
            }

            var index = e.ColumnIndex - 2;

            if (index >= 0)
            {
                e.Value = _data[e.RowIndex].Width;
                return;
            }
        }

        private void btnAuto_Click(object sender, EventArgs e)
        {
            _data[0x41].Width = 0x0E;
            _data[0x4F].Width = 0x0E;
            _data[0x55].Width = 0x0E;
            _data[0x4E].Width = 0x0E;

            var replacements = new List<Tuple<char, char, char>>
            {
                new Tuple<char, char, char>('É', '\u0026', 'E'), //&
                new Tuple<char, char, char>('Í', '\u0027', 'I'), //'
                new Tuple<char, char, char>('á', '\u003B', 'a'), //;
                new Tuple<char, char, char>('é', '\u005C', 'e'), //\
                new Tuple<char, char, char>('í', '\u005E', 'i'), //^
                new Tuple<char, char, char>('ó', '\u005F', 'o'), //_
                new Tuple<char, char, char>('ú', '\u0060', 'u'), //`
                new Tuple<char, char, char>('ü', '\u007B', 'u'), //{
                new Tuple<char, char, char>('ñ', '\u007D', 'n'), //}
                new Tuple<char, char, char>('¡', '\u007E', 'i'), //~
                new Tuple<char, char, char>('¿', '\u007F', '?'), //DEL
            };

            foreach (var (_, item2, item3) in replacements)
            {
                _data[item2].Width = _data[item3].Width;
            }

            FontCharsGridView.Invalidate();
        }
    }
}
