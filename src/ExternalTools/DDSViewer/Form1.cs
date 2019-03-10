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

namespace DDSViewer
{
    public partial class Form1 : Form
    {
        private List<string> _dds;
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
                _dds = new List<string>(Directory.GetFiles(folderBrowserDialog1.SelectedPath, "*.dds", SearchOption.AllDirectories));
                _dds.Sort();
                _index = 0;
                _count = _dds.Count;

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
            var currentFile = _dds[_index];
            Text = $"{currentFile} ({_index + 1}/{_count})";
            try
            {
                var dds = DirectXTexNet.TexHelper.Instance.LoadFromDDSFile(currentFile, DDS_FLAGS.NONE);
                var codec = DirectXTexNet.TexHelper.Instance.GetWICCodec(WICCodecs.PNG);

                var metadata = dds.GetMetadata();
                var decompressed = (metadata.Format != DXGI_FORMAT.B8G8R8A8_UNORM) ? dds.Decompress(DXGI_FORMAT.B8G8R8A8_UNORM) : dds;
                var image = decompressed.SaveToWICMemory(0, WIC_FLAGS.NONE, codec);

                imageBox1.Image = System.Drawing.Image.FromStream(image);
            }
            catch (Exception e)
            {
                imageBox1.Image = null;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (_file != null)
            {
                _file.WriteLine(_dds[_index]);
                _index++;
                if (_index == _dds.Count)
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
