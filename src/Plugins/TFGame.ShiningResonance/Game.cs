using System.Collections.Generic;
using System.Drawing;
using System.Text;
using TF.Core.Entities;

namespace TFGame.ShiningResonance
{
    public class Game : CRIWareGame.CRIWareGame
    {
        public override string Id => "f19142d4-d6bb-4fd1-bcfe-1cb29868df7b";
        public override string Name => "Shining Resonance Refrain";
        public override string Description => "Versión en inglés Steam (10/09/2018)";
        public override Image Icon => Resources.Icon; // https://www.deviantart.com/andonovmarko/art/Shining-Resonance-Refrain-Icon-v1-746462251
        public override int Version => 1;
        public override Encoding FileEncoding => Encoding.UTF8;

        public override GameFileContainer[] GetContainers(string path)
        {
            var result = new List<GameFileContainer>();

            var mtpFiles = new GameFileSearch
            {
                RelativePath = @"data\resource\mtpa_en",
                SearchPattern = "*.mtp",
                IsWildcard = true,
                RecursiveSearch = false,
                FileType = typeof(Files.Mtp.File),
            };

            var drg022 = new GameFileContainer
            {
                Path = @".\drg022.cpk",
                Type = ContainerType.CompressedFile
            };

            drg022.FileSearches.Add(mtpFiles);

            result.Add(drg022);
            return result.ToArray();
        }
    }
}
