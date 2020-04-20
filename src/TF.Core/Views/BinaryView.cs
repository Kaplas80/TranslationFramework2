using System;
using System.Windows.Forms;
using Be.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace TF.Core.Views
{
    public partial class BinaryView : DockContent
    {
        public event NewFileLoadedEventHandler ImportFile;
        public delegate void NewFileLoadedEventHandler(string fileName);
        public event SaveFileEventHandler ExportFile;
        public delegate void SaveFileEventHandler(string fileName);

        private string _fileName;
        private DynamicFileByteProvider _provider;

        public BinaryView(string fileName)
        {
            InitializeComponent();
            _fileName = fileName;
        }

        public void SetFileFilter(string filter)
        {
            openFileDialog1.Filter = filter;
            openFileDialog1.FileName = _fileName;
            saveFileDialog1.Filter = filter;
            saveFileDialog1.FileName = _fileName;
        }

        public void LoadData(string file)
        {
            if (_provider != null)
            {
                _provider.Dispose();
            }

            _provider = new DynamicFileByteProvider(file, true);
            hexBox1.ByteProvider = _provider;
        }

        protected virtual void OnNewFileLoaded(string selectedFile)
        {
            ImportFile?.Invoke(selectedFile);
        }

        protected virtual void OnSaveFile(string selectedFile)
        {
            ExportFile?.Invoke(selectedFile);
        }

        protected virtual void btnExportFile_Click(object sender, EventArgs e)
        {
            var dialogResult = saveFileDialog1.ShowDialog(this);
            if (dialogResult == DialogResult.OK)
            {
                OnSaveFile(saveFileDialog1.FileName);
            }
        }

        protected virtual void btnImportFile_Click(object sender, EventArgs e)
        {
            var dialogResult = openFileDialog1.ShowDialog(this);
            if (dialogResult == DialogResult.OK)
            {
                OnNewFileLoaded(openFileDialog1.FileName);
            }
        }

        private void BinaryView_FormClosing(object sender, FormClosingEventArgs e)
        {
            hexBox1.ByteProvider = null;
            if (_provider != null)
            {
                _provider.Dispose();
            }
            GC.Collect();
        }
    }
}
