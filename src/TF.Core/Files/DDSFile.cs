namespace TF.Core.Files
{
    using System;
    using System.IO;
    using DirectXTexNet;
    using Image = System.Drawing.Image;

    public class DDSFile : ImageFile
    {
        protected class TexMetadataView
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

        protected override string Filter => "Ficheros DDS (*.dds)|*.dds|Todos los ficheros (*.*)|*.*";

        protected ScratchImage _currentDDS;

        public DDSFile(string gameName, string path, string changesFolder) : base(gameName, path, changesFolder)
        {
        }

        ~DDSFile()
        {
            _currentDDS?.Dispose();
        }

        public override void ExportImage(string filename)
        {
            if (_currentDDS != null && !_currentDDS.IsDisposed)
            {
                _currentDDS.Dispose();
                _currentDDS = null;
            }
        
            _currentDDS = GetScratchImage();
            
            string directory = System.IO.Path.GetDirectoryName(filename);
            Directory.CreateDirectory(directory);

            _currentDDS.SaveToDDSFile(DDS_FLAGS.NONE, filename);

            _currentDDS.Dispose();
            _currentDDS = null;
        }

        protected virtual ScratchImage GetScratchImage()
        {
            string source = HasChanges ? ChangesFile : Path;
            ScratchImage img = TexHelper.Instance.LoadFromDDSFile(source, DDS_FLAGS.NONE);

            TexMetadata metadata = img.GetMetadata();

            if (TexHelper.Instance.IsCompressed(metadata.Format))
            {
                ScratchImage tmp = img.Decompress(DXGI_FORMAT.UNKNOWN);
                img.Dispose();
                img = tmp;
            }

            return img;
        }

        protected override Image GetDrawingImage()
        {
            if (_currentDDS != null && !_currentDDS.IsDisposed)
            {
                _currentDDS.Dispose();
            }
        
            _currentDDS = GetScratchImage();

            try
            {
                Guid codec = TexHelper.Instance.GetWICCodec(WICCodecs.PNG);

                UnmanagedMemoryStream imageStream = _currentDDS.SaveToWICMemory(0, WIC_FLAGS.NONE, codec);

                Image image = Image.FromStream(imageStream);

                imageStream.Close();
                imageStream?.Dispose();

                return image;
                
            }
            catch (Exception)
            {
                return null;
            }
        }

        protected override object GetImageProperties(object genericImage)
        {
            if (!(genericImage is ScratchImage scratchImage))
            {
                return null;
            }

            TexMetadata metadata = scratchImage.GetMetadata();
            var properties = new TexMetadataView(metadata);

            return properties;
        }
    }
}
