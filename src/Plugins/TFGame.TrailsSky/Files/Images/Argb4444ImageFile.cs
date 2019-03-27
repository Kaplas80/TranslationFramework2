namespace TFGame.TrailsSky.Files.Images
{

    public abstract class Argb4444ImageFile : AbstractImageFile
    {
        protected override int BytesPerPixel => 2;
        protected override string ImageFormat => "ARGB4444";

        protected Argb4444ImageFile(string path, string changesFolder) : base(path, changesFolder)
        {
        }

        protected override byte[] ConvertPixel(byte[] inputValues)
        {
            var value = (ushort)(inputValues[0] | inputValues[1] << 8);
            var result = new byte[4];

            var b = value & 0x0F;
            value >>= 4;
            var g = value & 0x0F;
            value >>= 4;
            var r = value & 0x0F;
            value >>= 4;
            var a = value;

            result[0] = (byte)(b * 0x11);
            result[1] = (byte)(g * 0x11);
            result[2] = (byte)(r * 0x11);
            result[3] = (byte)(a * 0x11);

            return result;
        }
    }
}
