﻿namespace TFGame.TrailsSky.Files.MSDT
{
    public class File2 : File
    {
        protected override byte[] SearchPattern => new byte[] { 0x00, 0x00, 0x00, 0x0E };
        public File2(string gameName, string path, string changesFolder, System.Text.Encoding encoding) : base(gameName, path, changesFolder, encoding)
        {
        }
    }
}
