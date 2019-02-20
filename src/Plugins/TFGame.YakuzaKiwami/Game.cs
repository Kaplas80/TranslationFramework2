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
            auth_w64_containers.Exclusions.Add("inst_auth.par");
            auth_w64_containers.FileSearches.Add(cmnSearch);
            //auth_w64_containers.FileSearches.Add(ddsSearch);

            result.AddRange(auth_w64_containers.GetContainers(path));
            
            var aiPopupSearch =
                new GameFileSearch
                {
                    RelativePath = ".",
                    SearchPattern = "ai_popup.bin",
                    IsWildcard = false,
                    RecursiveSearch = false
                };

            var wdr_par_c_common = new GameFileContainer
            {
                Path = @"data\wdr_par_c\common.par", Type = ContainerType.CompressedFile
            };
            wdr_par_c_common.FileSearches.Add(aiPopupSearch);

            result.Add(wdr_par_c_common);

            result.Sort();
            return result.ToArray();
        }
    }
}
