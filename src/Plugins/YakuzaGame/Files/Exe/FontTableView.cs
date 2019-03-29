using System;
using System.Drawing;
using System.Windows.Forms;
using Cyotek.Windows.Forms;
using DataGridViewNumericUpDownElements;
using DirectXTexNet;
using WeifenLuo.WinFormsUI.Docking;
using Image = System.Drawing.Image;

namespace YakuzaGame.Files.Exe
{
    public partial class FontTableView : DockContent
    {
        private CharacterInfo[] _data;
        private Image _originalFontImage;
        private Image _newFontImage;

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
                ReadOnly = true
            };
            FontCharsGridView.Columns.Add(colId);

            var colChr = new DataGridViewTextBoxColumn
            {
                Name = "colChr",
                HeaderText = "Chr",
                DefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.LightGray },
                ReadOnly = true
            };
            FontCharsGridView.Columns.Add(colChr);

            var column = new DataGridViewNumericUpDownColumn
            {
                Name = "colValue0",
                HeaderText = "TopLeft",
                DecimalPlaces = 3,
                DefaultCellStyle = new DataGridViewCellStyle {Alignment = DataGridViewContentAlignment.MiddleRight},
                Increment = new decimal(new [] {1, 0, 0, 131072}),
                Maximum = new decimal(new [] {2, 0, 0, 0}),
                Minimum = new decimal(new [] {2, 0, 0, -2147483648})
            };
            FontCharsGridView.Columns.Add(column);

            column = new DataGridViewNumericUpDownColumn
            {
                Name = "colValue1",
                HeaderText = "TopRight",
                DecimalPlaces = 3,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight },
                Increment = new decimal(new[] { 1, 0, 0, 131072 }),
                Maximum = new decimal(new[] { 2, 0, 0, 0 }),
                Minimum = new decimal(new[] { 2, 0, 0, -2147483648 })
            };
            FontCharsGridView.Columns.Add(column);

            column = new DataGridViewNumericUpDownColumn
            {
                Name = "colValue2",
                HeaderText = "MiddleLeft",
                DecimalPlaces = 3,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight },
                Increment = new decimal(new[] { 1, 0, 0, 131072 }),
                Maximum = new decimal(new[] { 2, 0, 0, 0 }),
                Minimum = new decimal(new[] { 2, 0, 0, -2147483648 })
            };
            FontCharsGridView.Columns.Add(column);

            column = new DataGridViewNumericUpDownColumn
            {
                Name = "colValue3",
                HeaderText = "MiddleRight",
                DecimalPlaces = 3,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight },
                Increment = new decimal(new[] { 1, 0, 0, 131072 }),
                Maximum = new decimal(new[] { 2, 0, 0, 0 }),
                Minimum = new decimal(new[] { 2, 0, 0, -2147483648 })
            };
            FontCharsGridView.Columns.Add(column);

            column = new DataGridViewNumericUpDownColumn
            {
                Name = "colValue4",
                HeaderText = "BottomLeft",
                DecimalPlaces = 3,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight },
                Increment = new decimal(new[] { 1, 0, 0, 131072 }),
                Maximum = new decimal(new[] { 2, 0, 0, 0 }),
                Minimum = new decimal(new[] { 2, 0, 0, -2147483648 })
            };
            FontCharsGridView.Columns.Add(column);

            column = new DataGridViewNumericUpDownColumn
            {
                Name = "colValue5",
                HeaderText = "BottomRight",
                DecimalPlaces = 3,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight },
                Increment = new decimal(new[] { 1, 0, 0, 131072 }),
                Maximum = new decimal(new[] { 2, 0, 0, 0 }),
                Minimum = new decimal(new[] { 2, 0, 0, -2147483648 })
            };
            FontCharsGridView.Columns.Add(column);

            FontCharsGridView.RowCount = 256;
        }

        private void btnLoadFont_Click(object sender, System.EventArgs e)
        {
            var result = openFileDialog1.ShowDialog(this);

            if (result == DialogResult.OK)
            {
                var font = openFileDialog1.FileName;

                try
                {
                    var dds = TexHelper.Instance.LoadFromDDSFile(font, DDS_FLAGS.NONE);
                    var codec = TexHelper.Instance.GetWICCodec(WICCodecs.PNG);

                    var decompressed = dds.Decompress(DXGI_FORMAT.B8G8R8A8_UNORM);
                    var image = decompressed.SaveToWICMemory(0, WIC_FLAGS.NONE, codec);

                    var btn = sender as Button;
                    if (btn.Name == "btnLoadOriginalFont")
                    {
                        _originalFontImage = Image.FromStream(image);
                    }

                    if (btn.Name == "btnLoadNewFont")
                    {
                        _newFontImage = Image.FromStream(image);
                    }

                    FontCharsGridView_SelectionChanged(null, null);
                }
                catch (Exception)
                {
                    MessageBox.Show("No se ha podido cargar la fuente", "ERROR", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                
            }
        }

        private void FontCharsGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (FontCharsGridView.SelectedRows.Count > 0)
            {
                var selectedRow = FontCharsGridView.SelectedRows[0];
                var index = (byte)selectedRow.Cells["colId"].Value;

                if (_originalFontImage != null)
                {
                    imgBoxOriginalChar.Image = GetCharImage(_originalFontImage, index);
                }

                if (_newFontImage != null)
                {
                    imgBoxNewChar.Image = GetCharImage(_newFontImage, index);
                }

                DrawText();
            }
        }

        private Bitmap GetCharImage(Image font, byte index)
        {
            var charWidth = font.Width / 16;
            var charHeight = font.Height / 16;

            var row = (index / 16) - 2;
            var col = index % 16;

            var p = new Point { X = col * charWidth, Y = row * charHeight };

            var rect = new Rectangle(p.X, p.Y, charWidth, charHeight);
            var bm = new Bitmap(charWidth, charHeight);

            using (var gr = Graphics.FromImage(bm))
            {
                var dest_rect = new Rectangle(0, 0, charWidth, charHeight);
                gr.DrawImage(font, dest_rect, rect, GraphicsUnit.Pixel);
            }

            return bm;
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (FontCharsGridView.SelectedRows.Count > 0)
            {
                var imgBox = sender as ImageBox;

                var charHeight = imgBox.Height;

                var selectedRow = FontCharsGridView.SelectedRows[0];
                var index = (byte)selectedRow.Cells["colId"].Value;

                float valor0 = 0;
                float valor1 = 0;
                float valor2 = 0;
                float valor3 = 0;
                float valor4 = 0;
                float valor5 = 0;

                if (imgBox.Name == "imgBoxOriginalChar")
                {
                    valor0 = (0.5f + _data[index].OriginalData[0]) * 0.333f * imgBox.Width;
                    valor1 = imgBox.Width - (0.5f + _data[index].OriginalData[1]) * 0.333f * imgBox.Width;
                    valor2 = (0.5f + _data[index].OriginalData[2]) * 0.333f * imgBox.Width;
                    valor3 = imgBox.Width - (0.5f + _data[index].OriginalData[3]) * 0.333f * imgBox.Width;
                    valor4 = (0.5f + _data[index].OriginalData[4]) * 0.333f * imgBox.Width;
                    valor5 = imgBox.Width - (0.5f + _data[index].OriginalData[5]) * 0.333f * imgBox.Width;
                }

                if (imgBox.Name == "imgBoxNewChar")
                {
                    valor0 = (0.5f + _data[index].Data[0]) * 0.333f * imgBox.Width;
                    valor1 = imgBox.Width - (0.5f + _data[index].Data[1]) * 0.333f * imgBox.Width;
                    valor2 = (0.5f + _data[index].Data[2]) * 0.333f * imgBox.Width;
                    valor3 = imgBox.Width - (0.5f + _data[index].Data[3]) * 0.333f * imgBox.Width;
                    valor4 = (0.5f + _data[index].Data[4]) * 0.333f * imgBox.Width;
                    valor5 = imgBox.Width - (0.5f + _data[index].Data[5]) * 0.333f * imgBox.Width;
                }
                
                using (var pen = new Pen(Color.Yellow, 1))
                {
                    e.Graphics.DrawLine(pen, valor0, 0, valor0, charHeight * 0.333f);
                    e.Graphics.DrawLine(pen, valor2, charHeight * 0.333f, valor2, charHeight * 0.666f);
                    e.Graphics.DrawLine(pen, valor4, charHeight * 0.666f, valor4, charHeight);
                }

                using (var pen = new Pen(Color.Blue, 1))
                {
                    e.Graphics.DrawLine(pen, valor1, 0, valor1, charHeight * 0.333f);
                    e.Graphics.DrawLine(pen, valor3, charHeight * 0.333f, valor3, charHeight * 0.666f);
                    e.Graphics.DrawLine(pen, valor5, charHeight * 0.666f, valor5, charHeight);
                }
            }
        }

        private void DrawText()
        {
            var txt = txtTest.Text;

            var bm = new Bitmap(pictureBox1.Width, pictureBox1.Height);

            if (_originalFontImage != null && !string.IsNullOrEmpty(txt))
            {
                var chars = txt.ToCharArray();

                using (var gr = Graphics.FromImage(bm))
                {
                    var y = 0f;
                    var x = 0f;
                    for (var i = 0; i < chars.Length; i++)
                    {
                        try
                        {
                            if (chars[i] == '\\' && chars[i + 1] == 'n')
                            {
                                y = y + _originalFontImage.Height / 16f;
                                i = i + 2;
                                x = DrawOriginalChar(gr, 0f, y, chars[i]);
                                continue;
                            }
                        }
                        catch (IndexOutOfRangeException)
                        {
                        }

                        try
                        {
                            x = DrawOriginalChar(gr, x, y, chars[i], chars[i - 1]);
                        }
                        catch (IndexOutOfRangeException)
                        {
                            try
                            {
                                x = DrawOriginalChar(gr, x, y, chars[i]);
                            }
                            catch (IndexOutOfRangeException)
                            {
                            }
                        }
                    }
                }
            }

            pictureBox1.Image = bm;

            bm = new Bitmap(pictureBox2.Width, pictureBox2.Height);

            if (_newFontImage != null && !string.IsNullOrEmpty(txt))
            {
                var chars = txt.ToCharArray();

                using (var gr = Graphics.FromImage(bm))
                {
                    var y = 0f;
                    var x = 0f;
                    for (var i = 0; i < chars.Length; i++)
                    {
                        try
                        {
                            if (chars[i] == '\\' && chars[i + 1] == 'n')
                            {
                                y = y + _newFontImage.Height / 16f;
                                i = i + 2;
                                x = DrawChar(gr, 0f, y, chars[i]);
                                continue;
                            }
                        }
                        catch (IndexOutOfRangeException)
                        {
                        }

                        try
                        {
                            x = DrawChar(gr, x, y, chars[i], chars[i - 1]);
                        }
                        catch (IndexOutOfRangeException)
                        {
                            try
                            {
                                x = DrawChar(gr, x, y, chars[i]);
                            }
                            catch (IndexOutOfRangeException)
                            {
                            }
                        }
                    }
                }
            }

            pictureBox2.Image = bm;
        }

        private float DrawChar(Graphics gr, float x, float y, char p1, char p2 = '\0')
        {
            if (_newFontImage != null)
            {
                var charWidth = _newFontImage.Width / 16;
                var charHeight = _newFontImage.Height / 16;

                var index1 = p1;
                var index2 = p2;

                if (index1 < 0 || index1 > 255 || index2 < 0 || index2 > 255)
                {
                    return x + charWidth;
                }

                var charInfo1 = _data[index1];
                var charInfo2 = _data[index2];

                var row = (index1 / 16) - 2;
                var col = index1 % 16;

                var p = new Point {X = col * charWidth, Y = row * charHeight};

                var rect = new Rectangle(p.X, p.Y, charWidth, charHeight);

                var factor =
                    Math.Min(
                        Math.Min(charInfo1.Data[0] + charInfo2.Data[1], charInfo1.Data[2] + charInfo2.Data[3]),
                        charInfo1.Data[4] + charInfo2.Data[5]);

                var preSpace = factor * 0.5f * charWidth;

                var currentX = x - preSpace;
                var destRect = new RectangleF(currentX, y, charWidth, charHeight);
                gr.DrawImage(_newFontImage, destRect, rect, GraphicsUnit.Pixel);

                return currentX + charWidth;
            }

            return x;
        }

        private float DrawOriginalChar(Graphics gr, float x, float y, char p1, char p2 = '\0')
        {
            if (_originalFontImage != null)
            {
                var charWidth = _originalFontImage.Width / 16;
                var charHeight = _originalFontImage.Height / 16;

                var index1 = p1;
                var index2 = p2;

                if (index1 < 0 || index1 > 255 || index2 < 0 || index2 > 255)
                {
                    return x + charWidth;
                }

                var charInfo1 = _data[index1];
                var charInfo2 = _data[index2];

                var row = (index1 / 16) - 2;
                var col = index1 % 16;

                var p = new Point { X = col * charWidth, Y = row * charHeight };

                var rect = new Rectangle(p.X, p.Y, charWidth, charHeight);

                var factor =
                    Math.Min(
                        Math.Min(charInfo1.OriginalData[0] + charInfo2.OriginalData[1], charInfo1.OriginalData[2] + charInfo2.OriginalData[3]),
                        charInfo1.OriginalData[4] + charInfo2.OriginalData[5]);

                var preSpace = factor * 0.5f * charWidth;

                var currentX = x - preSpace;
                var destRect = new RectangleF(currentX, y, charWidth, charHeight);
                gr.DrawImage(_originalFontImage, destRect, rect, GraphicsUnit.Pixel);

                return currentX + charWidth;
            }

            return x;
        }

        private void txtTest_TextChanged(object sender, EventArgs e)
        {
            DrawText();
        }

        private void FontCharsGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            imgBoxNewChar.Invalidate();

            DrawText();
        }

        private void FontCharsGridView_CellValuePushed(object sender, DataGridViewCellValueEventArgs e)
        {
            var index = e.ColumnIndex - 2;

            if (index >= 0)
            {
                _data[e.RowIndex][index] = 1.0f - Convert.ToSingle(e.Value);
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
                e.Value = 1.0f - _data[e.RowIndex][index];
                return;
            }
        }

        private void btnAutoAdjust_Click(object sender, EventArgs e)
        {
            const int pixelsSpace = 2;
            if (_newFontImage != null)
            {
                var result = MessageBox.Show("Esta opción eliminará los ajustes actuales. ¿Quieres continuar?",
                    "Ajuste automático", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.No)
                {
                    return;
                }

                for (var i = 0x00; i < 0x20; i++)
                {
                    _data[i].Data[0] = 0f;
                    _data[i].Data[1] = 0f;
                    _data[i].Data[2] = 0f;
                    _data[i].Data[3] = 0f;
                    _data[i].Data[4] = 0f;
                    _data[i].Data[5] = 0f;
                }

                _data[0x20].Data[0] = 0.5f;
                _data[0x20].Data[1] = 0.5f;
                _data[0x20].Data[2] = 0.5f;
                _data[0x20].Data[3] = 0.5f;
                _data[0x20].Data[4] = 0.5f;
                _data[0x20].Data[5] = 0.5f;

                for (var i = 0x21; i < 0x80; i++)
                {
                    var bitmap = GetCharImage(_newFontImage, (byte)i);

                    var tlPixel = Math.Min(bitmap.Width / 2,
                        GetMinPixel(bitmap, new Rectangle(0, 0, bitmap.Width / 2, bitmap.Height / 3)));
                    var mlPixel = Math.Min(bitmap.Width / 2,
                        GetMinPixel(bitmap, new Rectangle(0, bitmap.Height / 3, bitmap.Width / 2, bitmap.Height / 3)));
                    var blPixel = Math.Min(bitmap.Width / 2,
                        GetMinPixel(bitmap,
                            new Rectangle(0, 2 * bitmap.Height / 3, bitmap.Width / 2, bitmap.Height / 3)));

                    var trPixel = Math.Max(bitmap.Width / 2,
                        GetMaxPixel(bitmap, new Rectangle(bitmap.Width / 2, 0, bitmap.Width / 2, bitmap.Height / 3)));
                    var mrPixel = Math.Max(bitmap.Width / 2,
                        GetMaxPixel(bitmap, new Rectangle(bitmap.Width / 2, bitmap.Height / 3, bitmap.Width / 2, bitmap.Height / 3)));
                    var brPixel = Math.Max(bitmap.Width / 2,
                        GetMaxPixel(bitmap,
                            new Rectangle(bitmap.Width / 2, 2 * bitmap.Height / 3, bitmap.Width / 2,
                                bitmap.Height / 3)));

                    _data[i].Data[0] = (tlPixel - pixelsSpace) / (bitmap.Width / 2f);
                    _data[i].Data[1] = (bitmap.Width - (trPixel + pixelsSpace)) / (bitmap.Width / 2f);
                    _data[i].Data[2] = (mlPixel - pixelsSpace) / (bitmap.Width / 2f);
                    _data[i].Data[3] = (bitmap.Width - (mrPixel + pixelsSpace)) / (bitmap.Width / 2f);
                    _data[i].Data[4] = (blPixel - pixelsSpace) / (bitmap.Width / 2f);
                    _data[i].Data[5] = (bitmap.Width - (brPixel + pixelsSpace)) / (bitmap.Width / 2f);
                }

                for (var i = 0x80; i < 0xA1; i++)
                {
                    _data[i].Data[0] = 0f;
                    _data[i].Data[1] = 0f;
                    _data[i].Data[2] = 0f;
                    _data[i].Data[3] = 0f;
                    _data[i].Data[4] = 0f;
                    _data[i].Data[5] = 0f;
                }

                for (var i = 0xA1; i < 0x100; i++)
                {
                    var bitmap = GetCharImage(_newFontImage, (byte)i);

                    var tlPixel = Math.Min(bitmap.Width / 2,
                        GetMinPixel(bitmap, new Rectangle(0, 0, bitmap.Width / 2, bitmap.Height / 3)));
                    var mlPixel = Math.Min(bitmap.Width / 2,
                        GetMinPixel(bitmap, new Rectangle(0, bitmap.Height / 3, bitmap.Width / 2, bitmap.Height / 3)));
                    var blPixel = Math.Min(bitmap.Width / 2,
                        GetMinPixel(bitmap,
                            new Rectangle(0, 2 * bitmap.Height / 3, bitmap.Width / 2, bitmap.Height / 3)));

                    var trPixel = Math.Max(bitmap.Width / 2,
                        GetMaxPixel(bitmap, new Rectangle(bitmap.Width / 2, 0, bitmap.Width / 2, bitmap.Height / 3)));
                    var mrPixel = Math.Max(bitmap.Width / 2,
                        GetMaxPixel(bitmap, new Rectangle(bitmap.Width / 2, bitmap.Height / 3, bitmap.Width / 2, bitmap.Height / 3)));
                    var brPixel = Math.Max(bitmap.Width / 2,
                        GetMaxPixel(bitmap,
                            new Rectangle(bitmap.Width / 2, 2 * bitmap.Height / 3, bitmap.Width / 2,
                                bitmap.Height / 3)));

                    _data[i].Data[0] = (tlPixel - pixelsSpace) / (bitmap.Width / 2f);
                    _data[i].Data[1] = (bitmap.Width - (trPixel + pixelsSpace)) / (bitmap.Width / 2f);
                    _data[i].Data[2] = (mlPixel - pixelsSpace) / (bitmap.Width / 2f);
                    _data[i].Data[3] = (bitmap.Width - (mrPixel + pixelsSpace)) / (bitmap.Width / 2f);
                    _data[i].Data[4] = (blPixel - pixelsSpace) / (bitmap.Width / 2f);
                    _data[i].Data[5] = (bitmap.Width - (brPixel + pixelsSpace)) / (bitmap.Width / 2f);
                }

                FontCharsGridView.Invalidate();
                imgBoxNewChar.Invalidate();
                DrawText();
            }
        }

        private int GetMinPixel(Bitmap bitmap, Rectangle zone)
        {
            var result = int.MaxValue;

            for (var y = zone.Y; y < zone.Y + zone.Height; y++)
            {
                for (var x = zone.X; x < zone.X + zone.Width; x++)
                {
                    if (bitmap.GetPixel(x, y) != Color.FromArgb(0, 0, 0, 0))
                    {
                        result = Math.Min(result, x);
                        break;
                    }
                }
            }

            return result;
        }

        private int GetMaxPixel(Bitmap bitmap, Rectangle zone)
        {
            var result = int.MinValue;

            for (var y = zone.Y; y < zone.Y + zone.Height; y++)
            {
                for (var x = zone.X + zone.Width - 1; x >= zone.X; x--)
                {
                    if (bitmap.GetPixel(x, y) != Color.FromArgb(0, 0, 0, 0))
                    {
                        result = Math.Max(result, x);
                        break;
                    }
                }
            }

            return result;
        }
    }
}
