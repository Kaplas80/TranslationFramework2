using System;
using System.Windows.Forms;
using DirectXTexNet;
using WeifenLuo.WinFormsUI.Docking;

namespace TF.Core.Views
{
    public partial class DDSView : DockContent
    {
        private class TexMetadataView
        {
            private TexMetadata _metadata;

            public int Width => _metadata.Width;
            public int Height => _metadata.Height;
            public int Depth => _metadata.Depth;
            public int ArraySize => _metadata.ArraySize;
            public int MipLevels => _metadata.MipLevels;
            public TEX_MISC_FLAG MiscFlags => _metadata.MiscFlags;
            public TEX_MISC_FLAG2 MiscFlags2 => _metadata.MiscFlags2;
            public DXGI_FORMAT Format => _metadata.Format;
            public TEX_DIMENSION Dimension => _metadata.Dimension;

            public TexMetadataView(TexMetadata metadata)
            {
                _metadata = metadata;
            }
        }

        public event NewImageLoadedEventHandler NewImageLoaded;
        public delegate void NewImageLoadedEventHandler(string fileName);

        private ScratchImage _dds;

        protected DDSView()
        {
            InitializeComponent();
        }

        public DDSView(ThemeBase theme) : this()
        {
            dockPanel1.Theme = theme;

            dockPanel1.DocumentStyle = DocumentStyle.DockingSdi;
        }

        public void LoadImage(ScratchImage dds)
        {
            _dds = dds;

            var codec = DirectXTexNet.TexHelper.Instance.GetWICCodec(WICCodecs.PNG);

            var metadata = _dds.GetMetadata();
            var decompressed = (metadata.Format != DXGI_FORMAT.B8G8R8A8_UNORM) ? dds.Decompress(DXGI_FORMAT.B8G8R8A8_UNORM) : _dds;
            var image = decompressed.SaveToWICMemory(0, WIC_FLAGS.NONE, codec);

            imageBox1.Image = System.Drawing.Image.FromStream(image);
            var metadataView = new TexMetadataView(metadata);
            propertyGrid1.SelectedObject = metadataView;
        }

        protected virtual void OnNewImageLoaded(string selectedFile)
        {
            NewImageLoaded?.Invoke(selectedFile);
        }

        private void btnExportImage_Click(object sender, EventArgs e)
        {
            var dialogResult = saveFileDialog1.ShowDialog(this);
            if (dialogResult == DialogResult.OK)
            {
                _dds.SaveToDDSFile(DDS_FLAGS.NONE, saveFileDialog1.FileName);
            }
        }

        private void btnImportImage_Click(object sender, EventArgs e)
        {
            var dialogResult = openFileDialog1.ShowDialog(this);
            if (dialogResult == DialogResult.OK)
            {
                OnNewImageLoaded(openFileDialog1.FileName);
            }
        }
    }
}
