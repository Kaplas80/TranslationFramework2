namespace TF.Core.Files
{
    using System;
    using System.IO;
    using DirectXTexNet;
    using NeoSmart.StreamCompare;
    using Nito.AsyncEx.Synchronous;
    using Views;
    using Vortice.Direct3D;
    using Vortice.Direct3D11;
    using Vortice.DXGI;
    using WeifenLuo.WinFormsUI.Docking;
    using static Vortice.Direct3D11.D3D11;
    using static Vortice.DXGI.DXGI;

    // Esta clase representa ficheros DDS, pero los convierte a PNG para exportar e importar
    public class DDS2File : DDSFile
    {
        protected override string Filter => "Ficheros PNG (*.png)|*.png|Ficheros DDS (*.dds)|*.dds|Todos los ficheros (*.*)|*.*";

        private static readonly IDXGIFactory2 Factory;
        private static readonly FeatureLevel FeatureLevel;
        private static readonly ID3D11Device device;

        private static readonly FeatureLevel[] s_featureLevels = new[]
        {
            FeatureLevel.Level_11_1,
            FeatureLevel.Level_11_0,
            FeatureLevel.Level_10_1,
            FeatureLevel.Level_10_0
        };

        static DDS2File()
        {
            device = null;
            try
            {
                if (!CreateDXGIFactory1(out Factory).Failure)
                {
                    IDXGIAdapter1 adapter = GetHardwareAdapter();
                    if (adapter != null)
                    {
                        DeviceCreationFlags creationFlags = DeviceCreationFlags.BgraSupport;

                        if (D3D11CreateDevice(
                            adapter,
                            DriverType.Unknown,
                            creationFlags,
                            s_featureLevels,
                            out device, out FeatureLevel, out ID3D11DeviceContext tempContext).Failure)
                        {
                            // If the initialization fails, fall back to the WARP device.
                            // For more information on WARP, see:
                            // http://go.microsoft.com/fwlink/?LinkId=286690
                            D3D11CreateDevice(
                                null,
                                DriverType.Warp,
                                creationFlags,
                                s_featureLevels,
                                out device, out FeatureLevel, out tempContext).CheckError();
                        }

                        adapter.Dispose();
                    }
                }
            }
            catch (Exception)
            {
                device = null;
            }
        }

        public DDS2File(string gameName, string path, string changesFolder) : base(gameName, path, changesFolder)
        {
        }

        public override void Open(DockPanel panel)
        {
            string filename = System.IO.Path.GetFileNameWithoutExtension(Path);

            _view = new ImageView($"{filename}");
            _view.ImportImage += FormOnImportImage;
            _view.ExportImage += FormOnExportImage;
            _view.SetFileFilter(Filter);

            UpdateFormImage();
            _view.Show(panel, DockState.Document);
        }

        public override void ImportImage(string filename)
        {
            // Import
            // Si el fichero es un DDS, se llama al método base. Si es un PNG, se intenta convertir.
            if (System.IO.Path.GetExtension(filename).ToLowerInvariant() == ".png")
            {
                using (ScratchImage originalDds = DirectXTexNet.TexHelper.Instance.LoadFromDDSFile(Path, DDS_FLAGS.NONE))
                {
                    TexMetadata originalMetadata = originalDds.GetMetadata();

                    ScratchImage png = DirectXTexNet.TexHelper.Instance.LoadFromWICFile(filename, WIC_FLAGS.NONE);

                    var compareTask = CompareImagesAsync(originalDds, png);
                    var areEqual = compareTask.WaitAndUnwrapException();

                    if (areEqual)
                    {
                        png.Dispose();
                        return;
                    }

                    if (originalMetadata.MipLevels > 1)
                    {
                        ScratchImage aux = png.GenerateMipMaps(TEX_FILTER_FLAGS.DEFAULT, originalMetadata.MipLevels);
                        png.Dispose();
                        png = aux;
                    }

                    if (TexHelper.Instance.IsCompressed(originalMetadata.Format))
                    {
                        DXGI_FORMAT format = originalMetadata.Format;
                        TEX_COMPRESS_FLAGS flags = TEX_COMPRESS_FLAGS.DEFAULT | TEX_COMPRESS_FLAGS.PARALLEL;

                        if (TexHelper.Instance.IsSRGB(format))
                        {
                            flags |= TEX_COMPRESS_FLAGS.SRGB;
                        }

                        if (device == null || !IsDirectComputeCompatible(format))
                        {
                            using (ScratchImage newDds = png.Compress(format, flags, 0.5f))
                            {
                                newDds.SaveToDDSFile(DDS_FLAGS.NONE, ChangesFile);
                            }
                        }
                        else
                        {
                            try
                            {
                                using (ScratchImage newDds = png.Compress(device.NativePointer, format, flags, 1.0f))
                                {
                                    newDds.SaveToDDSFile(DDS_FLAGS.NONE, ChangesFile);
                                }
                            }
                            catch (Exception)
                            {
                                using (ScratchImage newDds = png.Compress(format, flags, 0.5f))
                                {
                                    newDds.SaveToDDSFile(DDS_FLAGS.NONE, ChangesFile);
                                }
                            }
                        }
                    }
                    else
                    {
                        png.SaveToDDSFile(DDS_FLAGS.NONE, ChangesFile);
                    }

                    png?.Dispose();
                }
            }
            else
            {
                base.ImportImage(filename);
            }
        }

        public override void ExportImage(string filename)
        {
            // Export
            // Igual que al importar, hay que decidir si es DDS o PNG
            if (System.IO.Path.GetExtension(filename).ToLowerInvariant() == ".png")
            {
                if (_currentDDS != null && !_currentDDS.IsDisposed)
                {
                    _currentDDS.Dispose();
                    _currentDDS = null;
                }

                _currentDDS = GetScratchImage();

                string directory = System.IO.Path.GetDirectoryName(filename);
                Directory.CreateDirectory(directory);

                Guid codec = DirectXTexNet.TexHelper.Instance.GetWICCodec(WICCodecs.PNG);
                TexMetadata metadata = _currentDDS.GetMetadata();

                if (TexHelper.Instance.IsCompressed(metadata.Format))
                {
                    using (ScratchImage decompressed = _currentDDS.Decompress(DXGI_FORMAT.UNKNOWN))
                    {
                        decompressed.SaveToWICFile(0, WIC_FLAGS.NONE, codec, filename);
                    }
                }
                else
                {
                    try
                    {
                        _currentDDS.SaveToWICFile(0, WIC_FLAGS.NONE, codec, filename);
                    }
                    catch (Exception)
                    {
                    }
                }

                _currentDDS.Dispose();
                _currentDDS = null;
            }
            else
            {
                base.ExportImage(filename);
            }
        }

        public override string GetExportFilename()
        {
            return $"{System.IO.Path.GetFileNameWithoutExtension(Path)}.png";
        }

        private async System.Threading.Tasks.Task<bool> CompareImagesAsync(ScratchImage dds, ScratchImage png)
        {
            Guid codec = DirectXTexNet.TexHelper.Instance.GetWICCodec(WICCodecs.PNG);
            TexMetadata metadata = dds.GetMetadata();

            UnmanagedMemoryStream ddsStream;
            if (TexHelper.Instance.IsCompressed(metadata.Format))
            {
                using (ScratchImage decompressed = dds.Decompress(DXGI_FORMAT.UNKNOWN))
                {
                    ddsStream = decompressed.SaveToWICMemory(0, WIC_FLAGS.NONE, codec);
                }
            }
            else
            {
                ddsStream = dds.SaveToWICMemory(0, WIC_FLAGS.NONE, codec);
            }

            UnmanagedMemoryStream pngStream = png.SaveToWICMemory(0, WIC_FLAGS.NONE, codec);

            var scompare = new StreamCompare();
            bool result = await scompare.AreEqualAsync(ddsStream, pngStream);
            ddsStream.Dispose();
            pngStream.Dispose();

            return result;
        }

        private static IDXGIAdapter1 GetHardwareAdapter()
        {
            IDXGIAdapter1 adapter = null;
            IDXGIFactory6 factory6 = Factory.QueryInterfaceOrNull<IDXGIFactory6>();
            if (factory6 != null)
            {
                for (int adapterIndex = 0;
                    factory6.EnumAdapterByGpuPreference(adapterIndex, GpuPreference.HighPerformance, out adapter).Success;
                    adapterIndex++)
                {
                    if (adapter == null)
                        continue;

                    AdapterDescription1 desc = adapter.Description1;

                    if ((desc.Flags & AdapterFlags.Software) != AdapterFlags.None)
                    {
                        // Don't select the Basic Render Driver adapter.
                        adapter.Dispose();
                        continue;
                    }

                    return adapter;
                }


                factory6.Dispose();
            }

            if (adapter == null)
            {
                for (int adapterIndex = 0;
                    Factory.EnumAdapters1(adapterIndex, out adapter).Success;
                    adapterIndex++)
                {
                    AdapterDescription1 desc = adapter.Description1;

                    if ((desc.Flags & AdapterFlags.Software) != AdapterFlags.None)
                    {
                        // Don't select the Basic Render Driver adapter.
                        adapter.Dispose();
                        continue;
                    }

                    return adapter;
                }
            }

            return adapter;
        }

        private static bool IsDirectComputeCompatible(DXGI_FORMAT format)
        {
            switch (format)
            {
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
