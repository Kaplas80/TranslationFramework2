using System.Collections.Generic;
using System.Drawing;
using TF.Core.Entities;
using TF.Core.Files;

namespace TFGame.YakuzaKiwami
{
    public class Game : YakuzaGame.Game
    {
        
        public override string Id => "7f8efe16-87fd-4f5a-a837-cacf3dde2852";
        public override string Name => "Yakuza Kiwami";
        public override string Description => "Build Id: 3645748";
        public override Image Icon => Resources.Icon; // https://www.deviantart.com/clarence1996/art/Yakuza-Kiwami-786854223
        public override int Version => 4;
        public override System.Text.Encoding FileEncoding => new Encoding();

        private GameFileContainer GetRoot()
        {
            var search =
                new GameFileSearch
                {
                    RelativePath = ".",
                    SearchPattern = "YakuzaKiwami.exe",
                    IsWildcard = false,
                    RecursiveSearch = false,
                    FileType = typeof(Files.Exe.File)
                };

            var c = new GameFileContainer
            {
                Path = @"media\",
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

            var dds2Search =
                new GameFileSearch
                {
                    RelativePath = ".",
                    SearchPattern = "*.dds",
                    IsWildcard = true,
                    RecursiveSearch = true,
                    FileType = typeof(DDSFile),
                };

            var sprite_c = new GameFileContainer
            { 
                Path = @"media\data\2dpar\sprite_c.par",
                Type  = ContainerType.CompressedFile,
            };
            sprite_c.FileSearches.Add(dds1Search);
            
            result.Add(sprite_c);

            var ui_c = new GameFileContainer
            {
                Path = @"media\data\2dpar\ui_c.par",
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
                    SearchPattern = "staffroll_d2_??.dds;ifc_jimaku_*.dds",
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
                    IsWildcard = true,
                    RecursiveSearch = true,
                    FileType = typeof(YakuzaGame.Files.Epmb.File)
                };

            var mailSearch =
                new GameFileSearch
                {
                    RelativePath = ".",
                    SearchPattern = "mail.bin_c",
                    IsWildcard = true,
                    RecursiveSearch = true,
                    FileType = typeof(YakuzaGame.Files.Mail.File)
                };

            var stringTblSearch =
                new GameFileSearch
                {
                    RelativePath = ".",
                    SearchPattern = "string_tbl.bin_c",
                    IsWildcard = true,
                    RecursiveSearch = true,
                    FileType = typeof(YakuzaGame.Files.StringTbl.File)
                };

            var tableSearch =
                new GameFileSearch
                {
                    RelativePath = ".",
                    SearchPattern = "ability.bin_c;battle_coefficient.bin_c;battle_deck_list.bin_c;caption.bin_c;complete_heat.bin_c;complete_majima.bin_c;explanation_main_scenario.bin_c;explanation_sub_story.bin_c;item.bin_c;tips_tutorial.bin_c",
                    //SearchPattern = "*.bin_c",
                    IsWildcard = true,
                    RecursiveSearch = false,
                    Exclusions = {"encounter_pupup_message.bin", "mail.bin", "string_tbl.bin"},
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
                    RelativePath = @".\hd2",
                    SearchPattern = "hd2_hankaku.dds",
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
                    FileType = typeof(YakuzaGame.Files.CmnBin.File),
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
                RelativePath = @"media\data\map_par",
                TypeSearch = ContainerType.CompressedFile,
                RecursiveSearch = false,
                SearchPattern = "st_kamuro.par_c;st_kawara_street.par_c"
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
                    SearchPattern = "baccarat_cpu.bin_c;baccarat_gallery_msg.bin_c;minigame_chohan_bakuto.bin_c;mesuking_*.bin_c;poker_com_*.bin_c",
                    IsWildcard = true,
                    RecursiveSearch = true,
                    FileType = typeof(YakuzaGame.Files.Table.File)
                };

            var minigame = new GameFileContainer
            {
                Path = @"media\data\minigame",
                Type = ContainerType.Folder
            };

            minigame.FileSearches.Add(tableSearch);

            var pocketCircuitSearch =
                new GameFileSearch
                {
                    RelativePath = "db",
                    SearchPattern = "*.bin_c",
                    IsWildcard = true,
                    RecursiveSearch = true,
                    FileType = typeof(YakuzaGame.Files.PocketCircuit.File)
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
                    SearchPattern = "lexus_minigame_release_retail.dll",
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
                Path = @"media\data\pausepar\chapter_c.par",
                Type = ContainerType.CompressedFile
            };

            chapter.FileSearches.Add(dds1Search);

            var tableSearch =
                new GameFileSearch
                {
                    RelativePath = ".",
                    SearchPattern = "extra.bin_c",
                    IsWildcard = false,
                    RecursiveSearch = false,
                    FileType = typeof(YakuzaGame.Files.Table.File)
                };

            var dds2Search =
                new GameFileSearch
                {
                    RelativePath = ".",
                    SearchPattern = "2d_mn_rom_continue.dds;2d_*.dds;head_pic_*.dds;kan_?.dds;ifc_jimaku_*.dds",
                    IsWildcard = true,
                    RecursiveSearch = true,
                    FileType = typeof(DDSFile)
                };

            var pause_c = new GameFileContainer
            {
                Path = @"media\data\pausepar\pause_c.par",
                Type = ContainerType.CompressedFile
            };

            pause_c.FileSearches.Add(tableSearch);
            pause_c.FileSearches.Add(dds2Search);

            var dds3Search =
                new GameFileSearch
                {
                    RelativePath = ".",
                    SearchPattern = "2d_jm_gp_*.dds",
                    IsWildcard = true,
                    RecursiveSearch = false,
                    FileType = typeof(DDSFile)
                };

            var tougijyo = new GameFileContainer
            {
                Path = @"media\data\pausepar\tougijyo.par",
                Type = ContainerType.CompressedFile
            };

            tougijyo.FileSearches.Add(dds3Search);

            var result = new List<GameFileContainer>();
            result.Add(chapter);
            result.Add(pause_c);
            result.Add(tougijyo);
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

        private GameFileContainer GetScenario()
        {
            var search =
                new GameFileSearch
                {
                    RelativePath = ".",
                    SearchPattern = "scenario2.*",
                    IsWildcard = true,
                    RecursiveSearch = false,
                    FileType = typeof(YakuzaGame.Files.Scenario.File)
                };

            var scenario = new GameFileContainer
            {
                Path = @"media\data\scenario",
                Type = ContainerType.Folder
            };

            scenario.FileSearches.Add(search);
            return scenario;
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
                    SearchPattern = "street_name_?.dat",
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
                    SearchPattern = "activity_list.bin_c;complete_minigame.bin_c;controller_explain.bin_c;correlation_person.bin_c;enc_boss_cmn_ability.bin_c;extra.bin_c;onedari_popup.bin_c;response_roulette.bin_c;tougijyo_mode.bin_c;tougijyo_participant.bin_c;tougijyo_realtime_quest.bin_c;tougijyo_string.bin_c;tutorial.bin_c;ultimate.bin_c;virtue_shop.bin_c",
                    //SearchPattern = "*.bin_c",
                    IsWildcard = true,
                    RecursiveSearch = false,
                    FileType = typeof(YakuzaGame.Files.Table.File)
                };

            var enemyNameSearch =
                new GameFileSearch
                {
                    RelativePath = ".",
                    SearchPattern = "enemy_name_all.bin_c",
                    IsWildcard = false,
                    RecursiveSearch = false,
                    FileType = typeof(YakuzaGame.Files.EnemyName.File)
                };

            var par = new GameFileContainer
            {
                Path = @"media\data\staypar\stay.par",
                Type = ContainerType.CompressedFile
            };

            par.FileSearches.Add(tableSearch);
            par.FileSearches.Add(enemyNameSearch);
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

            var wdr_msgSearch =
                new GameFileSearch
                {
                    RelativePath = ".",
                    SearchPattern = "*.msg",
                    IsWildcard = true,
                    RecursiveSearch = true,
                    FileType = typeof(YakuzaGame.Files.Msg.File)
                };

            var disposeStringSearch =
                new GameFileSearch
                {
                    RelativePath = ".",
                    SearchPattern = "dispose_string.bin",
                    IsWildcard = false,
                    RecursiveSearch = false,
                    FileType = typeof(YakuzaGame.Files.DisposeString.File)
                };

            var wdr_par = new GameFileContainer
            {
                Path = @"media\data\wdr_par_c\wdr.par",
                Type = ContainerType.CompressedFile
            };

            wdr_par.FileSearches.Add(wdr_barSearch);
            wdr_par.FileSearches.Add(wdr_restaurantSearch);
            wdr_par.FileSearches.Add(wdr_msgSearch);
            wdr_par.FileSearches.Add(disposeStringSearch);

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
            result.Add(GetScenario());
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

