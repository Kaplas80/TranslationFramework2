using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace TF.Core.Views
{
    public partial class FontView : DockContent
    {
        [DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont, IntPtr pdv, [In] ref uint pcFonts);

        [DllImport("gdi32.dll")]
        private static extern bool RemoveFontMemResourceEx(IntPtr handle);

        public event NewFontLoadedEventHandler NewFontLoaded;
        public delegate void NewFontLoadedEventHandler(string fileName);
        public event SaveFontEventHandler SaveFont;
        public delegate void SaveFontEventHandler(string fileName);

        private byte[] _font;
        private PrivateFontCollection _fontCollection;
        private IntPtr _fontHandle = IntPtr.Zero;

        protected FontView()
        {
            InitializeComponent();
            cbFontSize.SelectedIndex = 3;
            cbFontSize.SelectedIndexChanged += cbFontSize_SelectedIndexChanged;
        }

        public FontView(ThemeBase theme) : this()
        {
            dockPanel1.Theme = theme;

            dockPanel1.DocumentStyle = DocumentStyle.DockingSdi;
        }

        public void SetFileFilter(string filter)
        {
            openFileDialog1.Filter = filter;
            saveFileDialog1.Filter = filter;
        }

        public void LoadFont(byte[] font)
        {
            if (_fontHandle != IntPtr.Zero)
            {
                RemoveFontMemResourceEx(_fontHandle);
            }

            _font = font;
            _fontCollection = new PrivateFontCollection();

            var fontPtr = Marshal.AllocCoTaskMem(_font.Length);
            Marshal.Copy(_font, 0, fontPtr, _font.Length);
            uint dummy = 0;
            _fontCollection.AddMemoryFont(fontPtr, _font.Length);
            _fontHandle = AddFontMemResourceEx(fontPtr, (uint)_font.Length, IntPtr.Zero, ref dummy);

            Marshal.FreeCoTaskMem(fontPtr);

            RenderLabel();
        }

        private void RenderLabel()
        {
            var size = cbFontSize.SelectedItem.ToString();
            label1.Font = new Font(_fontCollection.Families[0], float.Parse(size), FontStyle.Regular, GraphicsUnit.Pixel, ((byte) (0)));
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
            if (_fontHandle != IntPtr.Zero)
            {
                RemoveFontMemResourceEx(_fontHandle);
            }
            GC.Collect();
        }
    }
}
