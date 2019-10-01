using System;
using System.IO;
using System.Runtime.InteropServices;
using DirectXTexNet;
using TF.Core.Files;
using TF.IO;
using Image = System.Drawing.Image;

namespace TFGame.HardcoreMecha.Files.CrunchDDS
{
    public class File : DDSFile
    {
        public File(string path, string changesFolder) : base(path, changesFolder)
        {
        }

        protected override Tuple<Image, object> GetImage()
        {
            if (!HasChanges)
            {
                var crunchedData = System.IO.File.ReadAllBytes(Path);
                var data = CrnDecompress(crunchedData);

                System.IO.File.WriteAllBytes(ChangesFile, data);
            }

            _currentDDS = TexHelper.Instance.LoadFromDDSFile(ChangesFile, DDS_FLAGS.NONE);

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
                var image = Image.FromStream(imageStream);

                var properties = new TexMetadataView(metadata);

                return new Tuple<Image, object>(image, properties);
            }
            catch (Exception e)
            {
                return new Tuple<Image, object>(null, null);
            }
            finally
            {
                decompressed.Dispose();
            }
        }

        public override void Rebuild(string outputFolder)
        {
            var outputFile = System.IO.Path.Combine(outputFolder, RelativePath);
            var dir = System.IO.Path.GetDirectoryName(outputFile);
            Directory.CreateDirectory(dir);

            if (System.IO.File.Exists(outputFile))
            {
                // Elimino el .crn para dejar solo el .dds
                System.IO.File.Delete(outputFile);
            }

            System.IO.File.Copy(HasChanges ? ChangesFile : Path, outputFile.Replace(".crn", ".dds"), true);
        }

        private static byte[] CrnDecompress(byte[] data)
        {
            IntPtr uncompressedData = default;
            try
            {
                var width = data[0x0C] << 8 | data[0x0D];
                var height = data[0x0E] << 8 | data[0x0F];
                var mipmaps = data[0x10];
                var format = data[0x12];

                var result = DecompressUnityCRN(data, data.Length, out uncompressedData, out var uncompressedSize);
                    
                if (result)
                {
                    var uncompressedBytes = new byte[uncompressedSize];
                    Marshal.Copy(uncompressedData, uncompressedBytes, 0, uncompressedSize);

                    var dds = InsertDdsHeader(uncompressedBytes, width, height, mipmaps, format);

                    return dds;
                }
                else
                {
                    throw new Exception("Unable to decompress crunched texture");
                }
            }
            finally
            {
                Marshal.FreeHGlobal(uncompressedData);
            }
        }

        private static byte[] InsertDdsHeader(byte[] data, int width, int height, int mipmaps, int format)
        {
            using (var ms = new MemoryStream())
            using (var output = new ExtendedBinaryWriter(ms))
            {
                output.WriteString("DDS ", false);
                output.Write(0x0000007C);
                output.Write(mipmaps > 1 ? 0x000A1007 : 0x00081007);
                output.Write(height);
                output.Write(width);
                output.Write(height * width); // pitch
                output.Write(0x00000000);
                output.Write(mipmaps);
                output.Skip(44);

                // PixelFormat
                output.Write(0x00000020);
                output.Write(0x00000004);
                output.WriteString(format == 0 ? "DXT1" : "DXT5", false);
                output.Write(0x00000000);
                output.Write(0x00000000);
                output.Write(0x00000000);
                output.Write(0x00000000);
                output.Write(0x00000000);

                output.Write(mipmaps > 1 ? 0x00401008 : 0x00001000);
                output.Write(0x00000000);
                output.Write(0x00000000);
                output.Write(0x00000000);
                output.Write(0x00000000);

                output.Write(data);
                return ms.ToArray();
            }
        }

        [DllImport("crunchunity_x64.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool DecompressUnityCRN(byte[] pSrc_file_data, int src_file_size, out IntPtr uncompressedData, out int uncompressedSize);
    }
}
