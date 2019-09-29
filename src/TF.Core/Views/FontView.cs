using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace TF.Core.Views
{
    public partial class FontView : DockContent
    {
        public event NewFontLoadedEventHandler NewFontLoaded;
        public delegate void NewFontLoadedEventHandler(string fileName);
        public event SaveFontEventHandler SaveFont;
        public delegate void SaveFontEventHandler(string fileName);

		private string _fileName;
        private FontFamily _fontFamily;

        public FontView(string fileName)
        {
            InitializeComponent();
            cbFontSize.SelectedIndex = 3;
            cbFontSize.SelectedIndexChanged += cbFontSize_SelectedIndexChanged;
            _fileName = fileName;
        }

        public void SetFileFilter(string filter)
        {
            openFileDialog1.Filter = filter;
            openFileDialog1.FileName = _fileName;
            saveFileDialog1.Filter = filter;
            saveFileDialog1.FileName = _fileName;
        }

        public unsafe void LoadFont(byte[] fontData)
        {
            var fonts = new PrivateFontCollection();
            fixed (byte* fontPtr = fontData)
            {
                fonts.AddMemoryFont((IntPtr) fontPtr, fontData.Length);
                _fontFamily = fonts.Families[0];
                RenderLabel();
            }

            fonts.Dispose();
        }

        private void RenderLabel()
        {
            var size = cbFontSize.SelectedItem.ToString();
            label1.Font = new Font(_fontFamily, float.Parse(size), FontStyle.Regular, GraphicsUnit.Pixel, ((byte) (0)));
            label1.Text = txtSample.Text;
        }

        protected virtual void OnNewImageLoaded(string selectedFile)
        {
            NewFontLoaded?.Invoke(selectedFile);
        }

        protected virtual void OnSaveImage(string selectedFile)
        {
            SaveFont?.Invoke(selectedFile);
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

        private void cbFontSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            RenderLabel();
        }

        private void txtSample_TextChanged(object sender, EventArgs e)
        {
            RenderLabel();
        }

        private void FontView_FormClosing(object sender, FormClosingEventArgs e)
        {
            GC.Collect();
        }
    }
}
