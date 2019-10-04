using System;
using System.IO;
using DirectXTexNet;

namespace DDSConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            var mode = args[0];
            if (mode == "-dds2png" && args.Length == 2) 
            {
                var input = Path.GetFullPath(args[1]);
                ConvertToPng(input);
            } 
            else if (mode == "-png2dds" && args.Length == 3) 
            {
                var input = Path.GetFullPath(args[1]);
                var output = Path.GetFullPath(args[2]);
                ConvertToDds(input, output);
            } 
        }

        private static void ConvertToPng(string inputFolder)
        {
            var ddsFiles = Directory.EnumerateFiles(inputFolder, "*.dds", SearchOption.AllDirectories);
            foreach (var file in ddsFiles) 
            {
                var name = Path.GetFileNameWithoutExtension(file);
                var directory = Path.GetDirectoryName(file);
                var relative = ".\\";
                if (directory != inputFolder)
                {
                    relative = directory.Remove(0, inputFolder.Length + 1);
                }

                var output = Path.Combine(inputFolder, relative, $"{name}.png");
                
                Console.WriteLine($"* {relative}\\{name}");

                DdsToPng(file, output);
            }
        }

        private static void DdsToPng(string input, string output)
        {
            using var dds = DirectXTexNet.TexHelper.Instance.LoadFromDDSFile(input, DDS_FLAGS.NONE);
            
            var codec = DirectXTexNet.TexHelper.Instance.GetWICCodec(WICCodecs.PNG);
            var metadata = dds.GetMetadata();

            if (IsCompressed(metadata.Format))
            {
                using var decompressed = dds.Decompress(DXGI_FORMAT.UNKNOWN);
                var metadata2 = decompressed.GetMetadata();
                decompressed.SaveToWICFile(0, WIC_FLAGS.NONE, codec, output);
                
            }
            else
            {
                dds.SaveToWICFile(0, WIC_FLAGS.NONE, codec, output);
            }
        }

        private static void ConvertToDds(string inputFolder, string outputFolder)
        {
            var ddsFiles = Directory.EnumerateFiles(inputFolder, "*.png", SearchOption.AllDirectories);
            foreach (var file in ddsFiles) 
            {
                var name = Path.GetFileNameWithoutExtension(file).Replace(".inv", string.Empty);
                var directory = Path.GetDirectoryName(file);
                var relative = ".\\";
                if (directory != inputFolder)
                {
                    relative = directory.Remove(0, inputFolder.Length + 1);
                }
                var inputDds = Path.Combine(inputFolder, relative, $"{name}.dds");
                var output = Path.Combine(outputFolder, relative, $"{name}.dds");

                Console.WriteLine($"* {relative}\\{name}");

                PngToDds(file, inputDds, output);
            }
        }

        private static void PngToDds(string input, string original, string output)
        {
            var directory = Path.GetDirectoryName(output);
            Directory.CreateDirectory(directory);

            var tmp = DirectXTexNet.TexHelper.Instance.LoadFromWICFile(input, WIC_FLAGS.NONE);
            using var originalDds = DirectXTexNet.TexHelper.Instance.LoadFromDDSFile(original, DDS_FLAGS.NONE);
            var originalMetadata = originalDds.GetMetadata();

            var png = input.Contains(".inv.") ? tmp.FlipRotate(TEX_FR_FLAGS.FLIP_VERTICAL) : tmp;

            if (originalMetadata.MipLevels > 1)
            {
                var aux = png.GenerateMipMaps(TEX_FILTER_FLAGS.DEFAULT, originalMetadata.MipLevels);
                png.Dispose();
                png = aux;
            }

            if (IsCompressed(originalMetadata.Format))
            {
                var format = originalMetadata.Format;
                if (format == DXGI_FORMAT.BC7_UNORM_SRGB)
                {
                    format = DXGI_FORMAT.BC7_UNORM;
                }
                using var newDds = png.Compress(format, TEX_COMPRESS_FLAGS.PARALLEL, 0.5f);
                newDds.SaveToDDSFile(DDS_FLAGS.NONE, output);
            }
            else
            {
                png.SaveToDDSFile(DDS_FLAGS.NONE, output);
            }
            
            png?.Dispose();
            tmp?.Dispose();
        }

        private static bool IsCompressed(DXGI_FORMAT format)
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
