namespace TFGame.TrailsSky.Files.Images
{

    public abstract class Argb4444ImageFile : AbstractImageFile
    {
        protected override int BytesPerPixel => 2;
        protected override string ImageFormat => "ARGB4444";

        protected Argb4444ImageFile(string gameName, string path, string changesFolder) : base(gameName, path, changesFolder)
        {
        }

        protected override byte[] ConvertPixelToBGRA8888(byte[] inputValues)
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

        protected override byte[] ConvertPixelToOriginalFormat(byte[] inputValues)
        {
            var result = new byte[2];

            var b = (inputValues[0] >> 4);
            var g = (inputValues[1] >> 4);
            var r = (inputValues[2] >> 4);
            var a = (inputValues[3] >> 4);

            result[0] = (byte)((g << 4) | b);
            result[1] = (byte)((a << 4) | r);

            return result;
        }
    }
}
