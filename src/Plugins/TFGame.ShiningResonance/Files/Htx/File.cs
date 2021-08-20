namespace TFGame.ShiningResonance.Files.Htx
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using DirectXTexNet;
    using TF.Core.Files;
    using TF.IO;
    using Image = System.Drawing.Image;

    public class File : DDSFile
    {
        public File(string gameName, string path, string changesFolder) : base(gameName, path, changesFolder)
        {
        }

        protected override void FormOnImportImage(string filename)
        {
            var data = AddHeaders(filename);

            System.IO.File.WriteAllBytes(ChangesFile, data);

            UpdateFormImage();
        }

        protected override ScratchImage GetScratchImage()
        {
            string source = HasChanges ? ChangesFile : Path;

            byte[] data = RemoveHeaders(source);

            IntPtr unmanagedPointer = Marshal.AllocHGlobal(data.Length);
            Marshal.Copy(data, 0, unmanagedPointer, data.Length);

            ScratchImage img = TexHelper.Instance.LoadFromDDSMemory(unmanagedPointer, data.Length, DDS_FLAGS.NONE);

            Marshal.FreeHGlobal(unmanagedPointer);

            TexMetadata metadata = img.GetMetadata();

            if (TexHelper.Instance.IsCompressed(metadata.Format))
            {
                ScratchImage tmp = img.Decompress(DXGI_FORMAT.UNKNOWN);
                img.Dispose();
                img = tmp;
            }

            return img;
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

                    output.Write(0x46535448);
                    output.Write(data.Length + 0x20);
                    output.Write(0x00000020);
                    output.Write(0x10000000);
                    output.Write(0x00000001);
                    output.Write(data.Length + 0x20);

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
                    output.Write(0x00000000);
                    output.Write(0x00000000);
                    output.Write(0x00000000);
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
