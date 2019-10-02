using System;
using DirectXTexNet;
using Image = System.Drawing.Image;

namespace TF.Core.Files
{
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

        protected override void FormOnSaveImage(string filename)
        {
            _currentDDS.SaveToDDSFile(DDS_FLAGS.NONE, filename);
        }

        protected override Tuple<Image, object> GetImage()
        {
            var source = HasChanges ? ChangesFile : Path;
            _currentDDS = TexHelper.Instance.LoadFromDDSFile(source, DDS_FLAGS.NONE);

            var codec = TexHelper.Instance.GetWICCodec(WICCodecs.PNG);

            var metadata = _currentDDS.GetMetadata();

            ScratchImage decompressed;
            try
            {
                decompressed = _currentDDS.Decompress(DXGI_FORMAT.UNKNOWN);
            }
            catch (ArgumentException e)
            {
                decompressed = _currentDDS;
            }

            try
            {
                var imageStream = decompressed.SaveToWICMemory(0, WIC_FLAGS.NONE, codec);

                decompressed.Dispose();

                var image = Image.FromStream(imageStream);

                var properties = new TexMetadataView(metadata);

                return new Tuple<Image, object>(image, properties);
            }
            catch (Exception e)
            {
                return new Tuple<Image, object>(null, null);
            }
        }
    }
}
