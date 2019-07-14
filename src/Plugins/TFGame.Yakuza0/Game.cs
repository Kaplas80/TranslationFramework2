using System.Collections.Generic;
using System.Drawing;
using System.Text;
using TF.Core.Entities;
using TF.Core.Files;

namespace TFGame.Yakuza0
{
    public class Game : YakuzaGame.Game
    {
        public override string Id => "193b8191-39e2-4ad7-b0bf-9cb413bb910f";
        public override string Name => "Yakuza 0";
        public override string Description => "Build Id: 3642285";

        public override Image Icon =>
            Resources.Icon; // https://www.deviantart.com/clarence1996/art/Yakuza-0-758095741

        public override int Version => 3;
        public override System.Text.Encoding FileEncoding => new Encoding();

        private GameFileContainer GetRoot()
        {
            var search =
                new GameFileSearch
                {
                    RelativePath = ".",
                    SearchPattern = "Yakuza0.exe",
                    IsWildcard = false,
                    RecursiveSearch = false,
                    FileType = typeof(Files.Exe.File)
                };

            var c = new GameFileContainer
            {
                Path = @"media",
                Type = ContainerType.Folder
            };

            c.FileSearches.Add(search);
            return c;
        }

        private IList<GameFileContainer> Get2dpar(string path)
        {
            var result = new List<GameFileContainer>();

            var dds1Search =
                new GameFileSearch
                {
                    RelativePath = ".",
                    SearchPattern = "2d_cf_*.dds",
                    IsWildcard = true,
                    RecursiveSearch = true,
                    FileType = typeof(DDSFile),
                };

            var sprite_c = new GameFileContainer
            {
                Path = @"media\data\2dpar\sprite_c.par",
                Type = ContainerType.CompressedFile,
            };
            sprite_c.FileSearches.Add(dds1Search);

            result.Add(sprite_c);

            var dds2Search =
                new GameFileSearch
                {
                    RelativePath = ".",
                    SearchPattern = "*.dds",
                    IsWildcard = true,
                    RecursiveSearch = true,
                    FileType = typeof(DDSFile),
                };

            var ui_c = new GameFileContainer
            {
                Path = @"media\data\2dpar\ui_e.par",
                Type = ContainerType.CompressedFile,
            };
            ui_c.FileSearches.Add(dds2Search);

            result.Add(ui_c);

            return result;
        }

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
                    FileType = typeof(YakuzaGame.Files.CmnBin.File)
                };

            var ddsSearch =
                new GameFileSearch
                {
                    RelativePath = ".",
                    SearchPattern = "staffroll*.dds;ycap_*.dds",
                    IsWildcard = true,
                    RecursiveSearch = true,
                    FileType = typeof(DDSFile)
                };

            var auth_w64_containers = new GameFileContainerSearch
            {
                RelativePath = @"media\data\auth_w64_e",
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
                    SearchPattern = "encounter_pupup_message.bin_c",
                    IsWildcard = false,
                    RecursiveSearch = false,
                    FileType = typeof(YakuzaGame.Files.Epmb.File)
                };

            var mailSearch =
                new GameFileSearch
                {
                    RelativePath = ".",
                    SearchPattern = "mail.bin_c",
                    IsWildcard = false,
                    RecursiveSearch = false,
                    FileType = typeof(YakuzaGame.Files.Mail.File)
                };

            var stringTblSearch =
                new GameFileSearch
                {
                    RelativePath = ".",
                    SearchPattern = "string_tbl.bin_c",
                    IsWildcard = false,
                    RecursiveSearch = false,
                    FileType = typeof(YakuzaGame.Files.StringTbl.File)
                };

            var tableSearch =
                new GameFileSearch
                {
                    RelativePath = ".",
                    SearchPattern =
                        "ability.bin_c;battle_deck_list.bin_c;caption.bin_c;complete_heat.bin_c;complete_shisho.bin_c;explanation_main_scenario.bin_c;explanation_sub_story.bin_c;item.bin_c;kiyaku.bin_c;money_result.bin_c;restaurant_menu.bin_c;tips_tutorial.bin_c",
                    //SearchPattern = "*.bin_?",
                    IsWildcard = true,
                    RecursiveSearch = false,
                    FileType = typeof(YakuzaGame.Files.Table.File)
                };

            var bootpar = new GameFileContainer
            {
                Path = @"media\data\bootpar\boot.par",
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
                    RelativePath = @".\hd",
                    SearchPattern = "hd_hankaku.dds",
                    IsWildcard = false,
                    RecursiveSearch = false,
                    FileType = typeof(DDSFile)
                };

            var fontpar = new GameFileContainer
            {
                Path = @"media\data\fontpar\font.par",
                Type = ContainerType.CompressedFile
            };
            fontpar.FileSearches.Add(ddsSearch);
            return fontpar;
        }

        private GameFileContainer GetHact()
        {
            var cmnSearch =
                new GameFileSearch
                {
                    RelativePath = ".",
                    SearchPattern = "cmn.bin",
                    IsWildcard = true,
                    RecursiveSearch = true,
                    FileType = typeof(YakuzaGame.Files.CmnBin.File)
                };

            var hact = new GameFileContainer
            {
                Path = @"media\data\hact.par",
                Type = ContainerType.CompressedFile
            };
            
            hact.FileSearches.Add(cmnSearch);

            return hact;
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
                    FileType = typeof(YakuzaGame.Files.Imb.File)
                };

            var map_par_containers = new GameFileContainerSearch
            {
                RelativePath = @"media\data\map_par_e",
                TypeSearch = ContainerType.CompressedFile,
                RecursiveSearch = false,
                SearchPattern = "map_cmn.par;st_kamuro.par;st_osaka.par"
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
                    SearchPattern = "baccarat_cpu.bin_c;baccarat_gallery_msg.bin_c;caba_item_list.bin_c;catfight_human_condition.bin_c;catfight_human_info.bin_c;catfight_information.bin_c;catfight_string.bin_c;catfight_string_matching.bin_c;minigame_chohan_bakuto.bin_c;fishing_bag_info.bin_c;fishing_dispose.bin_c;fishing_fish_info.bin_c;fishing_sao_info.bin_c;poker_com_*.bin_c",
                    IsWildcard = true,
                    RecursiveSearch = true,
                    FileType = typeof(YakuzaGame.Files.Table.File)
                };

            var ddsSearch =
                new GameFileSearch
                {
                    RelativePath = @".\teleclub",
                    SearchPattern = "*.dds",
                    IsWildcard = true,
                    RecursiveSearch = false,
                    FileType = typeof(DDSFile)
                };

            var minigame = new GameFileContainer
            {
                Path = @"media\data\minigame",
                Type = ContainerType.Folder
            };

            minigame.FileSearches.Add(tableSearch);
            minigame.FileSearches.Add(ddsSearch);

            var pocketCircuitSearch =
                new GameFileSearch
                {
                    RelativePath = "db",
                    SearchPattern = "*.bin_c",
                    IsWildcard = true,
                    RecursiveSearch = true,
                    FileType = typeof(Files.PocketCircuit.File)
                };

            var par = new GameFileContainer
            {
                Path = @"media\data\minigame\pokecir.par",
                Type = ContainerType.CompressedFile
            };

            par.FileSearches.Add(pocketCircuitSearch);

            var dllSearch =
                new GameFileSearch
                {
                    RelativePath = "w64",
                    SearchPattern = "cima_minigame_release_retail.dll",
                    IsWildcard = false,
                    RecursiveSearch = false,
                    FileType = typeof(Files.Dll.File)
                };

            var module = new GameFileContainer
            {
                Path = @"media\data\module",
                Type = ContainerType.Folder
            };

            module.FileSearches.Add(dllSearch);

            var result = new List<GameFileContainer>();
            result.Add(minigame);
            result.Add(par);
            result.Add(module);
            return result;
        }

        private IList<GameFileContainer> GetPause()
        {
            var ddsSearch =
                new GameFileSearch
                {
                    RelativePath = ".",
                    SearchPattern = "2d_k2_kyaba_cutin_*.dds",
                    IsWildcard = true,
                    RecursiveSearch = false,
                    FileType = typeof(DDSFile),
                };

            var cabaretIsland = new GameFileContainer
            {
                Path = @"media\data\pausepar_e\cabaret_island.par",
                Type = ContainerType.CompressedFile
            };

            cabaretIsland.FileSearches.Add(ddsSearch);

            var dds1Search =
                new GameFileSearch
                {
                    RelativePath = ".",
                    SearchPattern = "2d_mn_syotitle_??.dds",
                    IsWildcard = true,
                    RecursiveSearch = false,
                    FileType = typeof(DDSFile),
                };

            var chapter = new GameFileContainer
            {
                Path = @"media\data\pausepar_e\chapter.par",
                Type = ContainerType.CompressedFile
            };

            chapter.FileSearches.Add(dds1Search);

            ddsSearch =
                new GameFileSearch
                {
                    RelativePath = @"result\",
                    SearchPattern = "2d_ci_bu_rep*.dds",
                    IsWildcard = true,
                    RecursiveSearch = false,
                    FileType = typeof(DDSFile),
                };

            var findArms = new GameFileContainer
            {
                Path = @"media\data\pausepar_e\find_arms.par",
                Type = ContainerType.CompressedFile
            };

            findArms.FileSearches.Add(ddsSearch);

            ddsSearch =
                new GameFileSearch
                {
                    RelativePath = @".",
                    SearchPattern = "rule_*.dds",
                    IsWildcard = true,
                    RecursiveSearch = false,
                    FileType = typeof(DDSFile),
                };

            var minigame = new GameFileContainer
            {
                Path = @"media\data\pausepar_e\minigame.par",
                Type = ContainerType.CompressedFile
            };

            minigame.FileSearches.Add(ddsSearch);

            ddsSearch =
                new GameFileSearch
                {
                    RelativePath = @".",
                    SearchPattern = "p??_000.dds;staffroll.dds",
                    IsWildcard = true,
                    RecursiveSearch = false,
                    FileType = typeof(DDSFile),
                };

            var movie = new GameFileContainer
            {
                Path = @"media\data\pausepar_e\movie.par",
                Type = ContainerType.CompressedFile
            };

            movie.FileSearches.Add(ddsSearch);

            var pause = new GameFileContainer
            {
                Path = @"media\data\pausepar_e\pause.par",
                Type = ContainerType.CompressedFile
            };

            ddsSearch =
                new GameFileSearch
                {
                    RelativePath = @"ability\",
                    SearchPattern = "k_a_all.dds;k_master_??.dds;m_a_all.dds;m_master_??.dds",
                    IsWildcard = true,
                    RecursiveSearch = false,
                    FileType = typeof(DDSFile)
                };

            pause.FileSearches.Add(ddsSearch);

            ddsSearch =
                new GameFileSearch
                {
                    RelativePath = @"picture\",
                    SearchPattern = "2d_*.dds;head_pic_*.dds;kan_?.dds;rule_*.dds;ifc_jimaku_*.dds",
                    IsWildcard = true,
                    RecursiveSearch = true,
                    FileType = typeof(DDSFile)
                };

            pause.FileSearches.Add(ddsSearch);

            var pcircuit = new GameFileContainer
            {
                Path = @"media\data\pausepar_e\pcircuit.par",
                Type = ContainerType.CompressedFile
            };

            ddsSearch =
                new GameFileSearch
                {
                    RelativePath = @".",
                    SearchPattern = "2d_ci_item_battery_??.dds",
                    IsWildcard = true,
                    RecursiveSearch = false,
                    FileType = typeof(DDSFile)
                };

            pcircuit.FileSearches.Add(ddsSearch);

            var dds3Search =
                new GameFileSearch
                {
                    RelativePath = ".",
                    SearchPattern = "2d_jm_gp_*.dds;2d_yk_tg_style_??.dds",
                    IsWildcard = true,
                    RecursiveSearch = false,
                    FileType = typeof(DDSFile)
                };

            var tougijyo = new GameFileContainer
            {
                Path = @"media\data\pausepar_e\tougijyo.par",
                Type = ContainerType.CompressedFile
            };

            tougijyo.FileSearches.Add(dds3Search);

            var result = new List<GameFileContainer>();
            result.Add(cabaretIsland);
            result.Add(chapter);
            result.Add(findArms);
            result.Add(minigame);
            result.Add(movie);
            result.Add(pause);
            result.Add(pcircuit);
            result.Add(tougijyo);
            return result;
        }

        private GameFileContainer GetReactorpar()
        {
            var nameSearch =
                new GameFileSearch
                {
                    RelativePath = ".",
                    SearchPattern = "name_c.bin",
                    IsWildcard = true,
                    RecursiveSearch = false,
                    FileType = typeof(YakuzaGame.Files.Name.File)
                };

            var reactor = new GameFileContainer
            {
                Path = @"media\data\reactorpar\reactor_w64.par",
                Type = ContainerType.CompressedFile
            };

            reactor.FileSearches.Add(nameSearch);
            return reactor;
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
                    FileType = typeof(YakuzaGame.Files.Mfpb.File)
                };

            var soundpar = new GameFileContainer
            {
                Path = @"media\data\soundpar\sound.par",
                Type = ContainerType.CompressedFile
            };
            soundpar.FileSearches.Add(mfpSearch);

            return soundpar;
        }

        private GameFileContainer GetStage()
        {
            var streetNameSearch =
                new GameFileSearch
                {
                    RelativePath = ".",
                    SearchPattern = "street_name_c.dat",
                    IsWildcard = true,
                    RecursiveSearch = false,
                    FileType = typeof(YakuzaGame.Files.StreetName.File)
                };

            var stage = new GameFileContainer
            {
                Path = @"media\data\stage\w64\flag_data",
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
                    SearchPattern =
                        "activity_list.bin_c;battle_result.bin_c;caba_cast_info.bin_c;cabaret_island_area.bin_c;cabaret_island_shop.bin_c;controller_explain.bin_c;correlation_person.bin_c;money_island_shop.bin_c;money_island_tarent.bin_c;search_arms_agent.bin_c;search_arms_kind.bin_c;search_arms_location.bin_c;search_arms_result_picture.bin_c;tougijyo_realtime_quest.bin_c;tutorial.bin_c;ultimate.bin_c;virtue_shop.bin_c;",
                    IsWildcard = true,
                    RecursiveSearch = false,
                    FileType = typeof(YakuzaGame.Files.Table.File)
                };

            var par = new GameFileContainer
            {
                Path = @"media\data\staypar\stay.par",
                Type = ContainerType.CompressedFile
            };

            par.FileSearches.Add(tableSearch);
            return par;
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
                    FileType = typeof(YakuzaGame.Files.AiPopup.File)
                };

            var common_armsRepairSearch =
                new GameFileSearch
                {
                    RelativePath = "shop",
                    SearchPattern = "arms_repair.bin",
                    IsWildcard = false,
                    RecursiveSearch = false,
                    FileType = typeof(YakuzaGame.Files.ArmsRepair.File)
                };

            var common_blacksmithSearch =
                new GameFileSearch
                {
                    RelativePath = "shop",
                    SearchPattern = "blacksmith.bin",
                    IsWildcard = false,
                    RecursiveSearch = false,
                    FileType = typeof(YakuzaGame.Files.Blacksmith.File)
                };

            var common_presentSearch =
                new GameFileSearch
                {
                    RelativePath = "shop",
                    SearchPattern = "present.bin;send.bin;throw.bin",
                    IsWildcard = true,
                    RecursiveSearch = false,
                    FileType = typeof(YakuzaGame.Files.PresentSendThrow.File)
                };

            var common_saleSearch =
                new GameFileSearch
                {
                    RelativePath = "shop",
                    SearchPattern = "sale????.bin",
                    IsWildcard = true,
                    RecursiveSearch = false,
                    FileType = typeof(YakuzaGame.Files.Sale.File)
                };

            var wdr_par_c_common = new GameFileContainer
            {
                Path = @"media\data\wdr_par_c\common.par",
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

            var aiPopupSearch =
                new GameFileSearch
                {
                    RelativePath = "common",
                    SearchPattern = "ai_popup.bin",
                    IsWildcard = false,
                    RecursiveSearch = false,
                    FileType = typeof(YakuzaGame.Files.AiPopup.File)
                };

            var common_armsRepairSearch =
                new GameFileSearch
                {
                    RelativePath = "common\\shop",
                    SearchPattern = "arms_repair.bin",
                    IsWildcard = false,
                    RecursiveSearch = false,
                    FileType = typeof(YakuzaGame.Files.ArmsRepair.File)
                };

            var common_blacksmithSearch =
                new GameFileSearch
                {
                    RelativePath = "common\\shop",
                    SearchPattern = "blacksmith.bin",
                    IsWildcard = false,
                    RecursiveSearch = false,
                    FileType = typeof(YakuzaGame.Files.Blacksmith.File)
                };

            var common_presentSearch =
                new GameFileSearch
                {
                    RelativePath = "common\\shop",
                    SearchPattern = "present.bin;send.bin;throw.bin",
                    IsWildcard = true,
                    RecursiveSearch = false,
                    FileType = typeof(YakuzaGame.Files.PresentSendThrow.File)
                };

            var common_saleSearch =
                new GameFileSearch
                {
                    RelativePath = "common\\shop",
                    SearchPattern = "sale????.bin",
                    IsWildcard = true,
                    RecursiveSearch = false,
                    FileType = typeof(YakuzaGame.Files.Sale.File)
                };

            var wdr_barSearch =
                new GameFileSearch
                {
                    RelativePath = "shop",
                    SearchPattern = "bar????.bin",
                    IsWildcard = true,
                    RecursiveSearch = false,
                    FileType = typeof(YakuzaGame.Files.Bar.File)
                };

            var wdr_restaurantSearch =
                new GameFileSearch
                {
                    RelativePath = "shop",
                    SearchPattern = "restaurant????.bin;ex_shop????.bin;shop????.bin",
                    IsWildcard = true,
                    RecursiveSearch = false,
                    FileType = typeof(YakuzaGame.Files.Restaurant.File)
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
                    FileType = typeof(YakuzaGame.Files.Msg.File),
                    Exclusions =
                    {
                        "pac_STID_TE_0009.bin",
                        "pac_STID_TE_0010.bin"
                    }
                };

            var wdr_par = new GameFileContainer
            {
                Path = @"media\data\wdr_par_c\wdr.par",
                Type = ContainerType.CompressedFile
            };

            wdr_par.FileSearches.Add(aiPopupSearch);
            wdr_par.FileSearches.Add(common_armsRepairSearch);
            wdr_par.FileSearches.Add(common_blacksmithSearch);
            wdr_par.FileSearches.Add(common_presentSearch);
            wdr_par.FileSearches.Add(common_saleSearch);

            wdr_par.FileSearches.Add(wdr_barSearch);
            wdr_par.FileSearches.Add(wdr_restaurantSearch);
            wdr_par.FileSearches.Add(snitchSearch);
            wdr_par.FileSearches.Add(wdr_msgSearch);

            result.Add(wdr_par);

            return result;
        }

        public override GameFileContainer[] GetContainers(string path)
        {
            var result = new List<GameFileContainer>();

            result.Add(GetRoot());
            result.AddRange(Get2dpar(path));
            result.AddRange(GetAuth(path));
            result.Add(GetBootpar());
            result.Add(GetFontpar());
            result.Add(GetHact());
            result.AddRange(GetMappar(path));
            result.AddRange(GetMinigame());
            result.AddRange(GetPause());
            result.Add(GetReactorpar());
            result.Add(GetSoundpar());
            result.Add(GetStage());
            result.Add(GetStaypar());
            result.AddRange(GetWdrCommon());
            result.AddRange(GetWdr());

            result.Sort();
            return result.ToArray();
        }
    }
}
