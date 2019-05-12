using System.Collections.Generic;
using System.Drawing;
using TF.Core.Entities;
using TF.Core.Files;

namespace TFGame.YakuzaKiwami2
{
    public class Game : YakuzaGame.Game
    {

        public override string Id => "28bb0b85-2455-4cae-bb6a-fcc6b726c823";
        public override string Name => "Yakuza Kiwami 2";
        public override string Description => "Build Id: 3762252";
        public override Image Icon => Resources.Icon; // https://www.deviantart.com/andonovmarko/art/Yakuza-Kiwami-2-Icon-794434106
        public override int Version => 1;
        public override System.Text.Encoding FileEncoding => new Encoding();

        private GameFileContainer GetDb()
        {
            var search =
                new GameFileSearch
                {
                    RelativePath = @"en",
                    SearchPattern = "*.bin",
                    IsWildcard = true,
                    RecursiveSearch = false,
                    //FileType = typeof(DDSFile)
                };

            var folder = new GameFileContainer
            {
                Path = @"data\db.par",
                Type = ContainerType.CompressedFile
            };
            folder.FileSearches.Add(search);
            return folder;
        }

        private GameFileContainer GetFont()
        {
            var ddsSearch =
                new GameFileSearch
                {
                    RelativePath = @"en",
                    SearchPattern = "*.dds",
                    IsWildcard = true,
                    RecursiveSearch = false,
                    FileType = typeof(DDSFile)
                };

            var font = new GameFileContainer
            {
                Path = @"data\font",
                Type = ContainerType.Folder
            };
            font.FileSearches.Add(ddsSearch);
            return font;
        }

        private GameFileContainer GetUi()
        {
            var search =
                new GameFileSearch
                {
                    RelativePath = @"en",
                    SearchPattern = "*.dds",
                    IsWildcard = true,
                    RecursiveSearch = true,
                    FileType = typeof(DDSFile)
                };

            var folder = new GameFileContainer
            {
                Path = @"data\ui.par",
                Type = ContainerType.CompressedFile
            };
            folder.FileSearches.Add(search);
            return folder;
        }

        public override GameFileContainer[] GetContainers(string path)
        {
            var result = new List<GameFileContainer>();

            result.Add(GetDb());
            result.Add(GetFont());
            result.Add(GetUi());

            result.Sort();
            return result.ToArray();
        }
    }
}
