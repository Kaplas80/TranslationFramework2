﻿using System.IO;
using TF.Core.Files;
using TF.Core.Helpers;
using TFGame.PhoenixWrightTrilogy.Core;

namespace TFGame.PhoenixWrightTrilogy.Files
{
    public class EncryptedTextFile : TextFile
    {
        protected EncryptedTextFile(string gameName, string path, string changesFolder, System.Text.Encoding encoding) : base(gameName, path, changesFolder, encoding)
        {
        }

        public override bool Search(string searchString, string path = "")
        {
            if (string.IsNullOrEmpty(path))
            {
                path = Path;
            }
            var encryptedBytes = File.ReadAllBytes(path);
            var bytes = EncryptionManager.DecryptData(encryptedBytes);

            var pattern = FileEncoding.GetBytes(searchString.ToFullWidthChars());
            var encodedPattern = new byte[pattern.Length];

            using (var ms = new MemoryStream(pattern))
            using (var br = new BinaryReader(ms))
            using (var ms2 = new MemoryStream(encodedPattern))
            using (var bw = new BinaryWriter(ms2))
            {
                while (br.BaseStream.Position < br.BaseStream.Length)
                {
                    var chr = br.ReadUInt16();
                    bw.Write((ushort) (chr + 128));
                }
            }
            
            var searchHelper = new SearchHelper(encodedPattern);
            var index1 = searchHelper.Search(bytes);

            var index2 = -1;
            if (HasChanges)
            {
                bytes = File.ReadAllBytes(ChangesFile);
                pattern = System.Text.Encoding.Unicode.GetBytes(searchString);
                searchHelper = new SearchHelper(pattern);
                index2 = searchHelper.Search(bytes);
            }

            return index1 != -1 || index2 != -1;
        }
    }
}
