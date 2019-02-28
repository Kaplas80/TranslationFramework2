using System.Collections.Generic;
using System.Drawing;
using System.Text;
using TF.Core.Entities;
using TF.Core.Files.DDS;

namespace TFGame.Yakuza0
{
    public class Game : YakuzaCommon.Game
    {
        public override string Id => "193b8191-39e2-4ad7-b0bf-9cb413bb910f";
        public override string Name => "Yakuza 0";
        public override string Description => "Versión PC Steam sin DENUVO (lanzada el XX-XX-XXXX)";
        public override Image Icon => Resources.Icon; // https://www.deviantart.com/andonovmarko/art/Yakuza-0-Icon-750908182
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

            var snitchSearch =
                new GameFileSearch
                {
                    RelativePath = ".",
                    SearchPattern = "snitch.bin",
                    IsWildcard = false,
                    RecursiveSearch = false,
                    FileType = typeof(Files.Snitch.File)
                };

            var wdr_msgSearch =
                new GameFileSearch
                {
                    RelativePath = ".",
                    SearchPattern = "*.msg",
                    IsWildcard = true,
                    RecursiveSearch = true,
                    FileType = typeof(YakuzaCommon.Files.Msg.File),
                    Exclusions =
                    {
                        "pac_STID_ST_BOSS.bin", "pac_STID_ST_CRUISE.bin", "pac_STID_ST_EVENT.bin",
                        "pac_STID_ST_JUMU03.bin",
                        "pac_STID_ST_KENNSETSU.bin", "pac_STID_ST_KIRYU_APART.bin", "pac_STID_ST_KOUSOKU_CHASE.bin",
                        "pac_STID_ST_K_BAR.bin", "pac_STID_ST_K_BOWL.bin", "pac_STID_ST_K_CATFIGHT.bin",
                        "pac_STID_ST_K_CHUKA.bin", "pac_STID_ST_K_CLUBSEGA_01.bin", "pac_STID_ST_K_DEBORAH.bin",
                        "pac_STID_ST_K_GOUMONN.bin", "pac_STID_ST_K_KAZAMAGUMI.bin", "pac_STID_ST_K_RAMEN.bin",
                        "pac_STID_ST_K_RYOUTEI.bin", "pac_STID_ST_K_SAGAWA.bin", "pac_STID_ST_K_SNACK.bin",
                        "pac_STID_ST_K_SNACK_NAMASE.bin", "pac_STID_ST_K_SUGITA.bin", "pac_STID_ST_K_SUSHI.bin",
                        "pac_STID_ST_K_TEREKURA.bin", "pac_STID_ST_K_YAKINIKU.bin", "pac_STID_ST_K_YAMIISHA.bin",
                        "pac_STID_ST_LOVEHOTEL02.bin", "pac_STID_ST_O_BUKI.bin", "pac_STID_ST_O_CLUBSEGA.bin",
                        "pac_STID_ST_O_DOURAKU.bin", "pac_STID_ST_O_DUBORAYA.bin", "pac_STID_ST_O_GANKO.bin",
                        "pac_STID_ST_O_KAMIAN.bin", "pac_STID_ST_O_KIJIN.bin", "pac_STID_ST_O_ODYSSEY.bin",
                        "pac_STID_ST_O_RENTAL.bin", "pac_STID_ST_O_SOUKO.bin", "pac_STID_ST_O_STIJL.bin",
                        "pac_STID_ST_O_TEREKURA.bin", "pac_STID_ST_PARKING.bin", "pac_STID_ST_RINGER.bin",
                        "pac_STID_ST_SANCHU.bin", "pac_STID_ST_TOGYU.bin", "pac_STID_ST_TSUBAKI.bin",
                    }
                };

            var wdr_par = new GameFileContainer
            {
                Path = @"data\wdr_par_c\wdr.par",
                Type = ContainerType.CompressedFile
            };

            wdr_par.FileSearches.Add(wdr_barSearch);
            wdr_par.FileSearches.Add(wdr_restaurantSearch);
            wdr_par.FileSearches.Add(snitchSearch);
            wdr_par.FileSearches.Add(wdr_msgSearch);

            return wdr_par;
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
            result.Add(GetWdr());

            result.Sort();
            return result.ToArray();
        }
    }
}
