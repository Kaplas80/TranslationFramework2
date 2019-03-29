using System;
using System.IO;
using System.Runtime.InteropServices;
using DirectXTexNet;
using TF.Core.Files;
using TF.IO;
using Image = System.Drawing.Image;

namespace TFGame.ShiningResonance.Files.Htx
{
    class File : DDSFile
    {
        public File(string path, string changesFolder) : base(path, changesFolder)
        {
        }

        protected override void FormOnNewImageLoaded(string filename)
        {
            var data = AddHeaders(filename);

            System.IO.File.WriteAllBytes(ChangesFile, data);

            UpdateFormImage();
        }

        protected override Tuple<Image, object> GetImage()
        {
            var source = HasChanges ? ChangesFile : Path;

            var data = RemoveHeaders(source);

            var unmanagedPointer = Marshal.AllocHGlobal(data.Length);
            Marshal.Copy(data, 0, unmanagedPointer, data.Length);

            _currentDDS = TexHelper.Instance.LoadFromDDSMemory(unmanagedPointer, data.Length, DDS_FLAGS.NONE);
            
            Marshal.FreeHGlobal(unmanagedPointer);

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

            var imageStream = decompressed.SaveToWICMemory(0, WIC_FLAGS.NONE, codec);
            var image = Image.FromStream(imageStream);

            var properties = new TexMetadataView(metadata);
            return new Tuple<Image, object>(image, properties);
        }

        private static byte[] AddHeaders(string filename)
        {
            var data = System.IO.File.ReadAllBytes(filename);
            using (var ms = new MemoryStream())
            {
                using (var output = new ExtendedBinaryWriter(ms))
                {
                    output.Write(0x58455448);
                    output.Write(data.Length + 0x60);
                    output.Write(0x00000020);
                    output.Write(0x10000004);
                    output.Skip(0x10);

                    output.Write(0x48545346);
                    output.Write(data.Length + 0x20);
                    output.Write(0x00000020);
                    output.Write(0x10000000);
                    output.Write(0x00000001);
                    output.Write(data.Length + 0x20);
                    output.Skip(0x10);

                    output.Skip(0x28);

                    output.Write(data);

                    output.Write(0x43464F45);
                    output.Write(0x00000000);
                    output.Write(0x00000020);
                    output.Write(0x10000000);
                    output.Write(0x00000001);
                    output.Skip(0x0C);

                    output.Write(0x43464F45);
                    output.Write(0x00000000);
                    output.Write(0x00000020);
                    output.Write(0x10000000);
                    output.Write(0x00000000);
                    output.Skip(0x0C);
                }

                return ms.ToArray();
            }
        }

        private static byte[] RemoveHeaders(string filename)
        {
            var data = System.IO.File.ReadAllBytes(filename);
            using (var outputMs = new MemoryStream())
            {
                outputMs.Write(data, 0x60, data.Length - 0xA0);

                return outputMs.ToArray();
            }
        }
    }
}
