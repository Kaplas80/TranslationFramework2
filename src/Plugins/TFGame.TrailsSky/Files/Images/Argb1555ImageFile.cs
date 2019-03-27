namespace TFGame.TrailsSky.Files.Images
{

    public abstract class Argb1555ImageFile : AbstractImageFile
    {
        protected override int BytesPerPixel => 2;
        protected override string ImageFormat => "ARGB1555";

        protected Argb1555ImageFile(string path, string changesFolder) : base(path, changesFolder)
        {
        }

        protected override byte[] ConvertPixel(byte[] inputValues)
        {
            var value = (ushort)(inputValues[0] | inputValues[1] << 8);
            var result = new byte[4];

            var b = value & 0x1F;
            value >>= 5;
            var g = value & 0x1F;
            value >>= 5;
            var r = value & 0x1F;
            value >>= 5;
            var a = value;
            
            result[0] = (byte)((b << 3) | (b >> 2));
            result[1] = (byte)((g << 3) | (g >> 2));
            result[2] = (byte)((r << 3) | (r >> 2));
            result[3] = (byte)(a * 0xFF);

            return result;
        }
    }
}
