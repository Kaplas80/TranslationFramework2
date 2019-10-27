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

        protected override void FormOnSaveImage(string filename)
        {
            _currentDDS.SaveToDDSFile(DDS_FLAGS.NONE, filename);
        }

        protected override Tuple<Image, object> GetImage()
        {
            string source = HasChanges ? ChangesFile : Path;
            _currentDDS = TexHelper.Instance.LoadFromDDSFile(source, DDS_FLAGS.NONE);

            Guid codec = TexHelper.Instance.GetWICCodec(WICCodecs.PNG);

            TexMetadata metadata = _currentDDS.GetMetadata();

            if (IsCompressed(metadata.Format))
            {
                ScratchImage tmp = _currentDDS.Decompress(DXGI_FORMAT.UNKNOWN);
                _currentDDS.Dispose();
                _currentDDS = tmp;
            }

            try
            {
                UnmanagedMemoryStream imageStream = _currentDDS.SaveToWICMemory(0, WIC_FLAGS.NONE, codec);

                Image image = Image.FromStream(imageStream);

                imageStream.Close();
                imageStream?.Dispose();

                var properties = new TexMetadataView(metadata);

                return new Tuple<Image, object>(image, properties);
            }
            catch (Exception)
            {
                return new Tuple<Image, object>(null, null);
            }
        }

        protected static bool IsCompressed(DXGI_FORMAT format)
        {
            switch (format)
            {
                case DXGI_FORMAT.BC1_TYPELESS:
                case DXGI_FORMAT.BC1_UNORM:
                case DXGI_FORMAT.BC1_UNORM_SRGB:
                case DXGI_FORMAT.BC2_TYPELESS:
                case DXGI_FORMAT.BC2_UNORM:
                case DXGI_FORMAT.BC2_UNORM_SRGB:
                case DXGI_FORMAT.BC3_TYPELESS:
                case DXGI_FORMAT.BC3_UNORM:
                case DXGI_FORMAT.BC3_UNORM_SRGB:
                case DXGI_FORMAT.BC4_TYPELESS:
                case DXGI_FORMAT.BC4_UNORM:
                case DXGI_FORMAT.BC4_SNORM:
                case DXGI_FORMAT.BC5_TYPELESS:
                case DXGI_FORMAT.BC5_UNORM:
                case DXGI_FORMAT.BC5_SNORM:
                case DXGI_FORMAT.BC6H_TYPELESS:
                case DXGI_FORMAT.BC6H_UF16:
                case DXGI_FORMAT.BC6H_SF16:
                case DXGI_FORMAT.BC7_TYPELESS:
                case DXGI_FORMAT.BC7_UNORM:
                case DXGI_FORMAT.BC7_UNORM_SRGB:
                    return true;

                default:
                    return false;
            }
        }
    }
}
