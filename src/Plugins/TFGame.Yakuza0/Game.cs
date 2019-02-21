using System.Collections.Generic;
using System.Drawing;
using TF.Core.Entities;

namespace TFGame.Yakuza0
{
    public class Game : YakuzaCommon.Game
    {
        public override string Id => "193b8191-39e2-4ad7-b0bf-9cb413bb910f";
        public override string Name => "Yakuza 0";
        public override string Description => "Versión PC Steam sin DENUVO (lanzada el XX-XX-XXXX)";
        public override Image Icon => Resources.Icon; // https://www.deviantart.com/andonovmarko/art/Yakuza-0-Icon-750908182
        public override int Version => 1;

        public override GameFileContainer[] GetContainers(string path)
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

            var auth_w64_containers = new GameFileContainerSearch
            {
                RelativePath = @"data\auth_w64_e",
                TypeSearch = ContainerType.CompressedFile,
                RecursiveSearch = false,
                SearchPattern = "*.par"
            };
            auth_w64_containers.FileSearches.Add(cmnSearch);
            //auth_w64_containers.FileSearches.Add(ddsSearch);

            result.AddRange(auth_w64_containers.GetContainers(path));
            result.Sort();
            return result.ToArray();
        }
    }
}
