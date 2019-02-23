using System.Collections.Generic;
using System.Drawing;
using System.Text;
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
        public override Encoding FileEncoding => new YakuzaEncoding();

        public override GameFileContainer[] GetContainers(string path)
        {
            var result = new List<GameFileContainer>();

            var empbSearch =
                new GameFileSearch
                {
                    RelativePath = ".",
                    SearchPattern = "encounter_pupup_message.*",
                    IsWildcard = true,
                    RecursiveSearch = true,
                    FileType = typeof(YakuzaCommon.Files.Epmb.File)
                };

            var mailSearch =
                new GameFileSearch
                {
                    RelativePath = ".",
                    SearchPattern = "mail.*",
                    IsWildcard = true,
                    RecursiveSearch = true,
                    FileType = typeof(YakuzaCommon.Files.Mail.File)
                };

            var bootpar = new GameFileContainer
            {
                Path = @"data\bootpar\boot.par",
                Type = ContainerType.CompressedFile
            };
            bootpar.FileSearches.Add(empbSearch);
            bootpar.FileSearches.Add(mailSearch);

            result.Add(bootpar);

            result.Sort();
            return result.ToArray();
        }
    }
}
