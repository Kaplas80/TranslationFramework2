using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TF.Core.Entities;

namespace TFGame.YakuzaKiwami
{
    public class Game : YakuzaCommon.Game
    {
        public override string Id => "7f8efe16-87fd-4f5a-a837-cacf3dde2852";
        public override string Name => "Yakuza Kiwami";
        public override string Description => "Versión PC Steam sin DENUVO (lanzada el 19/02/2019)";
        public override Image Icon => Resources.Icon; // https://www.deviantart.com/andonovmarko/art/Yakuza-Kiwami-Icon-750908330
        public override int Version => 1;

        public override GameFileContainer[] Containers
        {
            get
            {
                var result = new List<GameFileContainer>();

                var cmnSearch =
                    new GameFileSearch
                    {
                        RelativePath = ".",
                        SearchPattern = "cmn.bin",
                        IsWildcard = true,
                        RecursiveSearch = true
                    };

                var ddsSearch =
                    new GameFileSearch
                    {
                        RelativePath = ".",
                        SearchPattern = "*.dds",
                        IsWildcard = true,
                        RecursiveSearch = true
                    };

                var hact = new GameFileContainer
                    { Path = "data\\hact.par", Type = ContainerType.CompressedFile };
                hact.FileSearches.Add(cmnSearch);
                hact.FileSearches.Add(ddsSearch);
                //result.Add(hact);

                var fontpar = new GameFileContainer
                    { Path = "data\\fontpar\\font.par", Type = ContainerType.CompressedFile };
                fontpar.FileSearches.Add(ddsSearch);
                result.Add(fontpar);

                result.Sort();
                return result.ToArray();
            }
        }
    }
}
