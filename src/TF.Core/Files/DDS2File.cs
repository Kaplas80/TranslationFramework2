namespace TF.Core.Files
{
    using System;
    using System.IO;
    using DirectXTexNet;
    using Views;
    using WeifenLuo.WinFormsUI.Docking;

    // Esta clase representa ficheros DDS, pero los convierte a PNG para exportar e importar
    public class DDS2File : DDSFile
    {
        protected override string Filter => "Ficheros PNG (*.png)|*.png|Todos los ficheros (*.*)|*.*";

        public DDS2File(string gameName, string path, string changesFolder) : base(gameName, path, changesFolder)
        {
        }

        public override void Open(DockPanel panel)
        {
            string filename = System.IO.Path.GetFileNameWithoutExtension(Path);

            _view = new ImageView($"{filename}.png");
            _view.ImportImage += FormOnImportImage;
            _view.ExportImage += FormOnExportImage;
            _view.SetFileFilter(Filter);

            UpdateFormImage();
            _view.Show(panel, DockState.Document);
        }

        public override void ImportImage(string filename)
        {
            // Import
            // Se le va a pasar un png y hay que convertirlo al formato de la DDS
            
            using (ScratchImage originalDds = DirectXTexNet.TexHelper.Instance.LoadFromDDSFile(Path, DDS_FLAGS.NONE))
            {
                TexMetadata originalMetadata = originalDds.GetMetadata();

                ScratchImage png = DirectXTexNet.TexHelper.Instance.LoadFromWICFile(filename, WIC_FLAGS.NONE);

                if (originalMetadata.MipLevels > 1)
                {
                    ScratchImage aux = png.GenerateMipMaps(TEX_FILTER_FLAGS.DEFAULT, originalMetadata.MipLevels);
                    png.Dispose();
                    png = aux;
                }

                if (IsCompressed(originalMetadata.Format))
                {
                    DXGI_FORMAT format = originalMetadata.Format;
                    if (format == DXGI_FORMAT.BC7_UNORM_SRGB)
                    {
                        format = DXGI_FORMAT.BC7_UNORM;
                    }

                    using (ScratchImage newDds = png.Compress(format, TEX_COMPRESS_FLAGS.PARALLEL, 0.5f))
                    {
                        newDds.SaveToDDSFile(DDS_FLAGS.NONE, ChangesFile);
                    }
                }
                else
                {
                    png.SaveToDDSFile(DDS_FLAGS.NONE, ChangesFile);
                }
            
                png?.Dispose();
            }
        }

        public override void ExportImage(string filename)
        {
            // Export
            // Hay que guardarlo como PNG
            if (_currentDDS != null && !_currentDDS.IsDisposed)
            {
                _currentDDS.Dispose();
            }
        
            _currentDDS = GetScratchImage();

            string directory = System.IO.Path.GetDirectoryName(filename);
            Directory.CreateDirectory(directory);

            Guid codec = DirectXTexNet.TexHelper.Instance.GetWICCodec(WICCodecs.PNG);
            TexMetadata metadata = _currentDDS.GetMetadata();

            if (IsCompressed(metadata.Format))
            {
                using (ScratchImage decompressed = _currentDDS.Decompress(DXGI_FORMAT.UNKNOWN))
                {
                    decompressed.SaveToWICFile(0, WIC_FLAGS.NONE, codec, filename);
                }
            }
            else
            {
                _currentDDS.SaveToWICFile(0, WIC_FLAGS.NONE, codec, filename);
            }
        }

        public override string GetExportFilename()
        {
            return $"{System.IO.Path.GetFileNameWithoutExtension(Path)}.png";
        }
    }
}
