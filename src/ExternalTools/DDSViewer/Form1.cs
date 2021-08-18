using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cyotek.Windows.Forms;
using DirectXTexNet;
using Image = System.Drawing.Image;

namespace DDSViewer
{
    public partial class Form1 : Form
    {
        private List<string> _images;
        private int _index;
        private StreamWriter _file;
        private int _count;

        public Form1()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                imageBox1.GridDisplayMode = ImageBoxGridDisplayMode.Client;
            }

            if (comboBox1.SelectedIndex == 1)
            {
                imageBox1.GridDisplayMode = ImageBoxGridDisplayMode.None;
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            var result = folderBrowserDialog1.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                var dds = new List<string>(Directory.GetFiles(folderBrowserDialog1.SelectedPath, "*.dds", SearchOption.AllDirectories));
                var png = new List<string>(Directory.GetFiles(folderBrowserDialog1.SelectedPath, "*.png", SearchOption.AllDirectories));

                _images = new List<string>(dds.Count + png.Count);

                _images.AddRange(dds);
                _images.AddRange(png);
                _images.Sort();
                _index = 0;
                _count = _images.Count;

                if (_count > 0)
                {
                    result = saveFileDialog1.ShowDialog(this);
                    if (result == DialogResult.OK)
                    {
                        _file = new StreamWriter(saveFileDialog1.FileName, true);

                        LoadImage();
                    }
                }
                else
                {
                    MessageBox.Show("No hay ficheros .dds");
                    Close();
                }
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_file != null)
            {
                _file.Close();
            }
        }

        private void LoadImage()
        {
            var currentFile = _images[_index];
            Text = $"{currentFile} ({_index + 1}/{_count})";
            try
            {
                if (Path.GetExtension(currentFile).ToLowerInvariant() == ".dds")
                {
                    var dds = DirectXTexNet.TexHelper.Instance.LoadFromDDSFile(currentFile, DDS_FLAGS.NONE);
                    var codec = DirectXTexNet.TexHelper.Instance.GetWICCodec(WICCodecs.PNG);

                    var metadata = dds.GetMetadata();
                    ScratchImage decompressed;
                    try
                    {
                        decompressed = dds.Decompress(DXGI_FORMAT.UNKNOWN);
                    }
                    catch (ArgumentException)
                    {
                        decompressed = dds;
                    }
                    
                    var image = decompressed.SaveToWICMemory(0, WIC_FLAGS.NONE, codec);

                    imageBox1.Image = System.Drawing.Image.FromStream(image);

                    dds.Dispose();
                    decompressed.Dispose();
                }
                else if (Path.GetExtension(currentFile).ToLowerInvariant() == ".png")
                {
                    imageBox1.Image = Image.FromFile(currentFile);
                }
                else
                {
                    imageBox1.Image = null;
                }
            }
            catch (Exception)
            {
                imageBox1.Image = null;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (_file != null)
            {
                _file.WriteLine(_images[_index]);
                _index++;
                if (_index == _images.Count)
                {
                    MessageBox.Show("Finalizado");
                    Close();
                }
                else
                {
                    LoadImage();
                }
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                if (_index > 0)
                {
                    _index--;
                    LoadImage();
                    return;
                }
            }

            if (e.KeyCode == Keys.Right)
            {
                if (_index < _count - 1)
                {
                    _index++;
                    LoadImage();
                    return;
                }
            }
        }
    }
}
