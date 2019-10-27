using System;
using System.Drawing;
using System.Windows.Forms;
using Cyotek.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace TF.Core.Views
{
    public partial class ImageView : DockContent
    {
        public event NewImageLoadedEventHandler ImportImage;
        public delegate void NewImageLoadedEventHandler(string fileName);
        public event SaveImageEventHandler ExportImage;
        public delegate void SaveImageEventHandler(string fileName);

        private Image _image;

        private string _fileName;

        public ImageView(string fileName)
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 1;
            _fileName = fileName;
        }

        public void SetFileFilter(string filter)
        {
            openFileDialog1.Filter = filter;
            openFileDialog1.FileName = _fileName;
            saveFileDialog1.Filter = filter;
            saveFileDialog1.FileName = _fileName;
        }

        public void LoadImage(Image image, object properties)
        {
            _image = image;

            imageBox1.Image = image;

            propertyGrid1.SelectedObject = properties;
        }

        protected virtual void OnNewImageLoaded(string selectedFile)
        {
            ImportImage?.Invoke(selectedFile);
        }

        protected virtual void OnSaveImage(string selectedFile)
        {
            ExportImage?.Invoke(selectedFile);
        }

        protected virtual void btnExportImage_Click(object sender, EventArgs e)
        {
            var dialogResult = saveFileDialog1.ShowDialog(this);
            if (dialogResult == DialogResult.OK)
            {
                OnSaveImage(saveFileDialog1.FileName);
            }
        }

        protected virtual void btnImportImage_Click(object sender, EventArgs e)
        {
            var dialogResult = openFileDialog1.ShowDialog(this);
            if (dialogResult == DialogResult.OK)
            {
                OnNewImageLoaded(openFileDialog1.FileName);
            }
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

        private void ImageView_FormClosing(object sender, FormClosingEventArgs e)
        {
            _image?.Dispose();
            GC.Collect();
        }
    }
}
