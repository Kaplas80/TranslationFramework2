using System;
using System.Drawing;
using System.Windows.Forms;
using Cyotek.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace TF.Core.Views
{
    public partial class ImageView : DockContent
    {
        public event NewImageLoadedEventHandler NewImageLoaded;
        public delegate void NewImageLoadedEventHandler(string fileName);
        public event SaveImageEventHandler SaveImage;
        public delegate void SaveImageEventHandler(string fileName);

        private Image _image;

        protected ImageView()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 1;
        }

        public ImageView(ThemeBase theme) : this()
        {
            dockPanel1.Theme = theme;

            dockPanel1.DocumentStyle = DocumentStyle.DockingSdi;
        }

        public void SetFileFilter(string filter)
        {
            openFileDialog1.Filter = filter;
            saveFileDialog1.Filter = filter;
        }

        public void LoadImage(Image image, object properties)
        {
            _image = image;

            imageBox1.Image = image;

            propertyGrid1.SelectedObject = properties;
        }

        protected virtual void OnNewImageLoaded(string selectedFile)
        {
            NewImageLoaded?.Invoke(selectedFile);
        }

        protected virtual void OnSaveImage(string selectedFile)
        {
            SaveImage?.Invoke(selectedFile);
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
            _image.Dispose();
            GC.Collect();
        }
    }
}
