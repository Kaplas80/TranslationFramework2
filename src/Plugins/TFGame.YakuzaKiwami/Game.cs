using System.Collections.Generic;
using System.Drawing;
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
            var result = new List<GameFileContainer>();

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

            result.AddRange(auth_w64_containers.GetContainers(path));

            auth_w64_containers = new GameFileContainerSearch
            {
                RelativePath = @"data\auth_w64_j",
                TypeSearch = ContainerType.CompressedFile,
                RecursiveSearch = false,
                SearchPattern = "*.par"
            };
            auth_w64_containers.Exclusions.Add("inst_auth.par");
            auth_w64_containers.FileSearches.Add(cmnSearch);
            auth_w64_containers.FileSearches.Add(ddsSearch);

            result.AddRange(auth_w64_containers.GetContainers(path));

            return result;
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

            var tableSearch =
                new GameFileSearch
                {
                    RelativePath = ".",
                    SearchPattern = "ability.bin_?;battle_coefficient.bin_?;battle_deck_list.bin_?;caption.bin_?;complete_heat.bin_?;complete_majima.bin_?;explanation_main_scenario.bin_?;explanation_sub_story.bin_?;item.bin_?;tips_tutorial.bin_?",
                    //SearchPattern = "*.bin_?",
                    IsWildcard = true,
                    RecursiveSearch = false,
                    Exclusions = {"encounter_pupup_message.bin", "mail.bin", "string_tbl.bin"},
                    FileType = typeof(YakuzaCommon.Files.Table.File)
                };

            var bootpar = new GameFileContainer
            {
                Path = @"data\bootpar\boot.par",
                Type = ContainerType.CompressedFile
            };
            bootpar.FileSearches.Add(empbSearch);
            bootpar.FileSearches.Add(mailSearch);
            bootpar.FileSearches.Add(stringTblSearch);
            bootpar.FileSearches.Add(tableSearch);
            return bootpar;
        }

        private GameFileContainer GetFontpar()
        {
            var ddsSearch =
                new GameFileSearch
                {
                    RelativePath = ".",
                    SearchPattern = "*.dds",
                    IsWildcard = true,
                    RecursiveSearch = true,
                    FileType = typeof(DDSFile)
                };

            var fontpar = new GameFileContainer
            {
                Path = @"data\fontpar\font.par",
                Type = ContainerType.CompressedFile
            };
            fontpar.FileSearches.Add(ddsSearch);
            return fontpar;
        }

        private IList<GameFileContainer> GetMappar(string path)
        {
            var result = new List<GameFileContainer>();

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

            result.AddRange(map_par_containers.GetContainers(path));

            map_par_containers = new GameFileContainerSearch
            {
                RelativePath = @"data\map_par_j",
                TypeSearch = ContainerType.CompressedFile,
                RecursiveSearch = false,
                SearchPattern = "st_kamuro.par;st_kawara_street.par"
            };
            map_par_containers.FileSearches.Add(imbSearch);

            result.AddRange(map_par_containers.GetContainers(path));

            return result;
        }

        private IList<GameFileContainer> GetMinigame()
        {
            var tableSearch =
                new GameFileSearch
                {
                    RelativePath = ".",
                    SearchPattern = "baccarat_cpu.bin_?;baccarat_gallery_msg.bin_?;minigame_chohan_bakuto.bin_?;mesuking_*.bin_?;poker_com_*.bin_?",
                    //SearchPattern = "*.bin_?",
                    IsWildcard = true,
                    RecursiveSearch = true,
                    FileType = typeof(YakuzaCommon.Files.Table.File)
                };

            var minigame = new GameFileContainer
            {
                Path = @"data\minigame",
                Type = ContainerType.Folder
            };

            minigame.FileSearches.Add(tableSearch);

            var pocketCircuitSearch =
                new GameFileSearch
                {
                    RelativePath = "db",
                    SearchPattern = "*.bin_?",
                    IsWildcard = true,
                    RecursiveSearch = true,
                    FileType = typeof(YakuzaKiwami.Files.PocketCircuit.File)
                };

            var par = new GameFileContainer
            {
                Path = @"data\minigame\pokecir.par",
                Type = ContainerType.CompressedFile
            };

            par.FileSearches.Add(pocketCircuitSearch);

            var result = new List<GameFileContainer>();
            result.Add(minigame);
            result.Add(par);
            return result;
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

        private IList<GameFileContainer> GetWdrCommon()
        {
            var result = new List<GameFileContainer>();

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

            result.Add(wdr_par_c_common);

            wdr_par_c_common = new GameFileContainer
            {
                Path = @"data\wdr_par_j\common.par",
                Type = ContainerType.CompressedFile
            };
            wdr_par_c_common.FileSearches.Add(aiPopupSearch);
            wdr_par_c_common.FileSearches.Add(common_armsRepairSearch);
            wdr_par_c_common.FileSearches.Add(common_blacksmithSearch);
            wdr_par_c_common.FileSearches.Add(common_presentSearch);
            wdr_par_c_common.FileSearches.Add(common_saleSearch);

            result.Add(wdr_par_c_common);

            return result;
        }

        private IList<GameFileContainer> GetWdr()
        {
            var result = new List<GameFileContainer>();

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

            var wdr_msgSearch =
                new GameFileSearch
                {
                    RelativePath = ".",
                    SearchPattern = "*.msg",
                    IsWildcard = true,
                    RecursiveSearch = true,
                    FileType = typeof(YakuzaCommon.Files.Msg.File)
                };

            var wdr_par = new GameFileContainer
            {
                Path = @"data\wdr_par_c\wdr.par",
                Type = ContainerType.CompressedFile
            };

            wdr_par.FileSearches.Add(wdr_barSearch);
            wdr_par.FileSearches.Add(wdr_restaurantSearch);
            wdr_par.FileSearches.Add(wdr_msgSearch);

            result.Add(wdr_par);

            wdr_par = new GameFileContainer
            {
                Path = @"data\wdr_par_j\wdr.par",
                Type = ContainerType.CompressedFile
            };

            wdr_par.FileSearches.Add(wdr_barSearch);
            wdr_par.FileSearches.Add(wdr_restaurantSearch);
            wdr_par.FileSearches.Add(wdr_msgSearch);

            result.Add(wdr_par);

            return result;
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

        private GameFileContainer GetStaypar()
        {
            var tableSearch =
                new GameFileSearch
                {
                    RelativePath = ".",
                    SearchPattern = "activity_list.bin_?;complete_minigame.bin_?;controller_explain.bin_?;correlation_person.bin_?;enc_boss_cmn_ability.bin_?;extra.bin_?;onedari_popup.bin_?;response_roulette.bin_?;tougijyo_mode.bin_?;tougijyo_participant.bin_?;tougijyo_realtime_quest.bin_?;tougijyo_string.bin_?;tutorial.bin_?;ultimate.bin_?;virtue_shop.bin_?",
                    //SearchPattern = "*.bin_?",
                    IsWildcard = true,
                    RecursiveSearch = false,
                    FileType = typeof(YakuzaCommon.Files.Table.File)
                };

            var par = new GameFileContainer
            {
                Path = @"data\staypar\stay.par",
                Type = ContainerType.CompressedFile
            };
            
            par.FileSearches.Add(tableSearch);
            return par;
        }

        public override GameFileContainer[] GetContainers(string path)
        {
            var result = new List<GameFileContainer>();

            //result.AddRange(GetAuth(path));
            result.Add(GetBootpar());
            //result.Add(GetFontpar());
            //result.AddRange(GetMappar(path));
            result.AddRange(GetMinigame());
            //result.Add(GetReactorpar());
            //result.Add(GetSoundpar());
            //result.Add(GetStage());
            result.Add(GetStaypar());
            //result.AddRange(GetWdrCommon());
            //result.AddRange(GetWdr());

            result.Sort();
            return result.ToArray();
        }
    }
}

