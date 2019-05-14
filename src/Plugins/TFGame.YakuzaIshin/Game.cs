using System.Collections.Generic;
using System.Drawing;
using TF.Core.Entities;
using TF.Core.Files;

namespace TFGame.YakuzaIshin
{
    public class Game : YakuzaGame.Game
    {
        
        public override string Id => "c3a0df5a-a3af-49bf-b604-c1161b5736bc";
        public override string Name => "Ryū ga Gotoku Ishin!";
        public override string Description => "BLJM61149";
        public override Image Icon => Resources.Icon; // https://en.wikipedia.org/wiki/Yakuza_Ishin
        public override int Version => 1;
        public override System.Text.Encoding FileEncoding => new Encoding();


        private IList<GameFileContainer> GetWdr()
        {
            var result = new List<GameFileContainer>();

            var wdr_msgSearch =
                new GameFileSearch
                {
                    RelativePath = ".",
                    SearchPattern = "*.msg",
                    IsWildcard = true,
                    RecursiveSearch = true,
                    FileType = typeof(YakuzaGame.Files.Msg.File)
                };

            var wdr_par = new GameFileContainer
            {
                Path = @"data\wdr_par\wdr.par",
                Type = ContainerType.CompressedFile
            };

            wdr_par.FileSearches.Add(wdr_msgSearch);

            result.Add(wdr_par);

            return result;
        }

        public override GameFileContainer[] GetContainers(string path)
        {
            var result = new List<GameFileContainer>();

            result.AddRange(GetWdr());

            result.Sort();
            return result.ToArray();
        }
    }
}

