using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TF.Core.Entities;
using TF.Core.Files.DDS;

namespace TFGame.YakuzaKiwami
{
    public class Game : YakuzaCommon.Game
    {
        
        public override string Id => "7f8efe16-87fd-4f5a-a837-cacf3dde2852";
        public override string Name => "Yakuza Kiwami";
        public override string Description => "Versión PC Steam sin DENUVO (lanzada el 19/02/2019)";
        public override Image Icon => Resources.Icon; // https://www.deviantart.com/andonovmarko/art/Yakuza-Kiwami-Icon-750908330
        public override int Version => 1;
        public override System.Text.Encoding FileEncoding => new Encoding();

        private IList<GameFileContainer> GetAuth(string path)
        {
            var cmnSearch =
                new GameFileSearch
                {
                    RelativePath = ".",
                    SearchPattern = "cmn.bin",
                    IsWildcard = true,
                    RecursiveSearch = true,
                    FileType = typeof(YakuzaCommon.Files.CmnBin.File)
                };

            var ddsSearch =
                new GameFileSearch
                {
                    RelativePath = ".",
                    SearchPattern = "*.dds",
                    IsWildcard = true,
                    RecursiveSearch = true,
                    FileType = typeof(DDSFile)
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
            auth_w64_containers.FileSearches.Add(ddsSearch);

            return auth_w64_containers.GetContainers(path);
        }

        private GameFileContainer GetBootpar()
        {
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

            var stringTblSearch =
                new GameFileSearch
                {
                    RelativePath = ".",
                    SearchPattern = "string_tbl.bin_?",
                    IsWildcard = true,
                    RecursiveSearch = true,
                    FileType = typeof(YakuzaCommon.Files.StringTbl.File)
                };

            var bootpar = new GameFileContainer
            {
                Path = @"data\bootpar\boot.par",
                Type = ContainerType.CompressedFile
            };
            bootpar.FileSearches.Add(empbSearch);
            bootpar.FileSearches.Add(mailSearch);
            bootpar.FileSearches.Add(stringTblSearch);
            return bootpar;
        }

        private IList<GameFileContainer> GetMappar(string path)
        {
            var imbSearch =
                new GameFileSearch
                {
                    RelativePath = ".",
                    SearchPattern = "*.imb",
                    IsWildcard = true,
                    RecursiveSearch = true,
                    FileType = typeof(YakuzaCommon.Files.Imb.File)
                };

            var map_par_containers = new GameFileContainerSearch
            {
                RelativePath = @"data\map_par",
                TypeSearch = ContainerType.CompressedFile,
                RecursiveSearch = false,
                SearchPattern = "st_kamuro.par_c;st_kawara_street.par_c"
            };
            map_par_containers.FileSearches.Add(imbSearch);

            return map_par_containers.GetContainers(path);
        }

        private GameFileContainer GetSoundpar()
        {
            var mfpSearch =
                new GameFileSearch
                {
                    RelativePath = ".",
                    SearchPattern = "*.mfp",
                    IsWildcard = true,
                    RecursiveSearch = true,
                    FileType = typeof(YakuzaCommon.Files.Mfpb.File)
                };

            var soundpar = new GameFileContainer
            {
                Path = @"data\soundpar\sound.par",
                Type = ContainerType.CompressedFile
            };
            soundpar.FileSearches.Add(mfpSearch);

            return soundpar;
        }

        private GameFileContainer GetWdrCommon()
        {
            var aiPopupSearch =
                new GameFileSearch
                {
                    RelativePath = ".",
                    SearchPattern = "ai_popup.bin",
                    IsWildcard = false,
                    RecursiveSearch = false,
                    FileType = typeof(YakuzaCommon.Files.AiPopup.File)
                };

            var common_armsRepairSearch =
                new GameFileSearch
                {
                    RelativePath = "shop",
                    SearchPattern = "arms_repair.bin",
                    IsWildcard = false,
                    RecursiveSearch = false,
                    FileType = typeof(YakuzaCommon.Files.SimpleSubtitle.File)
                };

            var common_blacksmithSearch =
                new GameFileSearch
                {
                    RelativePath = "shop",
                    SearchPattern = "blacksmith.bin",
                    IsWildcard = false,
                    RecursiveSearch = false,
                    FileType = typeof(YakuzaCommon.Files.Blacksmith.File)
                };

            var common_presentSearch =
                new GameFileSearch
                {
                    RelativePath = "shop",
                    SearchPattern = "present.bin;send.bin;throw.bin",
                    IsWildcard = true,
                    RecursiveSearch = false,
                    FileType = typeof(YakuzaCommon.Files.PresentSendThrow.File)
                };

            var common_saleSearch =
                new GameFileSearch
                {
                    RelativePath = "shop",
                    SearchPattern = "sale????.bin",
                    IsWildcard = true,
                    RecursiveSearch = false,
                    FileType = typeof(YakuzaCommon.Files.Sale.File)
                };

            var wdr_par_c_common = new GameFileContainer
            {
                Path = @"data\wdr_par_c\common.par",
                Type = ContainerType.CompressedFile
            };
            wdr_par_c_common.FileSearches.Add(aiPopupSearch);
            wdr_par_c_common.FileSearches.Add(common_armsRepairSearch);
            wdr_par_c_common.FileSearches.Add(common_blacksmithSearch);
            wdr_par_c_common.FileSearches.Add(common_presentSearch);
            wdr_par_c_common.FileSearches.Add(common_saleSearch);

            return wdr_par_c_common;
        }

        private GameFileContainer GetWdr()
        {
            var wdr_barSearch =
                new GameFileSearch
                {
                    RelativePath = "shop",
                    SearchPattern = "bar????.bin",
                    IsWildcard = true,
                    RecursiveSearch = false,
                    FileType = typeof(YakuzaCommon.Files.Bar.File)
                };

            var wdr_restaurantSearch =
                new GameFileSearch
                {
                    RelativePath = "shop",
                    SearchPattern = "restaurant????.bin;ex_shop????.bin;shop????.bin",
                    IsWildcard = true,
                    RecursiveSearch = false,
                    FileType = typeof(YakuzaCommon.Files.Restaurant.File)
                };

            var wdr_par = new GameFileContainer
            {
                Path = @"data\wdr_par_c\wdr.par",
                Type = ContainerType.CompressedFile
            };

            wdr_par.FileSearches.Add(wdr_barSearch);
            wdr_par.FileSearches.Add(wdr_restaurantSearch);

            return wdr_par;
        }

        private GameFileContainer GetReactorpar()
        {
            var nameSearch =
                new GameFileSearch
                {
                    RelativePath = ".",
                    SearchPattern = "name_?.bin",
                    IsWildcard = true,
                    RecursiveSearch = false,
                    FileType = typeof(YakuzaCommon.Files.Name.File)
                };

            var reactor = new GameFileContainer
            {
                Path = @"data\reactorpar\reactor_w64.par",
                Type = ContainerType.CompressedFile
            };

            reactor.FileSearches.Add(nameSearch);
            return reactor;
        }

        private GameFileContainer GetStage()
        {
            var streetNameSearch =
                new GameFileSearch
                {
                    RelativePath = ".",
                    SearchPattern = "street_name_?.dat",
                    IsWildcard = true,
                    RecursiveSearch = false,
                    FileType = typeof(YakuzaCommon.Files.StreetName.File)
                };

            var stage = new GameFileContainer
            {
                Path = @"data\stage\w64\flag_data",
                Type = ContainerType.Folder
            };

            stage.FileSearches.Add(streetNameSearch);
            return stage;
        }

        public override GameFileContainer[] GetContainers(string path)
        {
            var result = new List<GameFileContainer>();

            //result.AddRange(GetAuth(path));
            result.Add(GetBootpar());
            //result.AddRange(GetMappar(path));
            //result.Add(GetReactorpar());
            //result.Add(GetSoundpar());
            //result.Add(GetStage());
            //result.Add(GetWdrCommon());
            //result.Add(GetWdr());

            result.Sort();
            return result.ToArray();
        }
    }
}

