using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using TF.Core.Entities;
using TF.Core.Files;

namespace TFGame.TheMissing
{
    public class Game : UnityGame.Game
    {
        public override string Id => "880de844-b89d-42a8-b7c5-3361d887e983";
        public override string Name => "The MISSING: J.J. Macfield and the Island of Memories";
        public override string Description => "Steam Build Id: 3174849";
        public override Image Icon => Resources.Icon; // https://www.deviantart.com/clarence1996/art/The-MISSING-J-J-Macfield-atIoM-768659719
        public override int Version => 1;
        public override System.Text.Encoding FileEncoding => Encoding.UTF8;

        public override GameFileContainer[] GetContainers(string path)
        {
            var result = new List<GameFileContainer>();

            var txtSearch = new GameFileSearch()
            {
                RelativePath = @".",
                SearchPattern = "msg????en.txt",
                IsWildcard = true,
                RecursiveSearch = false,
                FileType = typeof(Files.Txt.File)
            };

            var ddsSearch = new GameFileSearch()
            {
                RelativePath = @".",
                SearchPattern = "stamp??.tex.dds;load.text.dds;photo_06.tex.dds;save.tex.dds;window.tex.dds",
                IsWildcard = true,
                RecursiveSearch = true,
                FileType = typeof(DDSFile)
            };

            var ttfSearch = new GameFileSearch()
            {
                RelativePath = @".",
                SearchPattern = "TT_NewCinemaB-D.ttf",
                IsWildcard = true,
                RecursiveSearch = true,
                FileType = typeof(TTFFile)
            };

            var phoneMessageSizesSearch = new GameFileSearch()
            {
                RelativePath = @".",
                SearchPattern = "resources_00001.-13",
                IsWildcard = true,
                RecursiveSearch = true,
                FileType = typeof(Files.PhoneBox.File)
            };

            var resources = new GameFileContainer
            {
                Path = @"TheMISSING_Data\resources.assets",
                Type = ContainerType.CompressedFile
            };

            resources.FileSearches.Add(txtSearch);
            resources.FileSearches.Add(ddsSearch);
            resources.FileSearches.Add(ttfSearch);
            resources.FileSearches.Add(phoneMessageSizesSearch);

            result.Add(resources);

            var graffitiSearch = new GameFileSearch()
            {
                RelativePath = @".",
                SearchPattern = "OBJ_Graffiti_01.tex.dds",
                IsWildcard = true,
                RecursiveSearch = true,
                FileType = typeof(DDSFile)
            };

            var sharedAssets44 = new GameFileContainer
            {
                Path = @"TheMISSING_Data\sharedassets44.assets",
                Type = ContainerType.CompressedFile
            };

            sharedAssets44.FileSearches.Add(graffitiSearch);

            result.Add(sharedAssets44);

            return result.ToArray();
        }
    }
}
