using System.Collections.Generic;
using System.Drawing;
using TF.Core.Entities;

namespace TFGame.YakuzaIshin
{
    public class Game : YakuzaGame.Game
    {
        
        public override string Id => "c3a0df5a-a3af-49bf-b604-c1161b5736bc";
        public override string Name => "Ryū ga Gotoku Ishin!";
        public override string Description => "BLJM61149";
        public override Image Icon => Resources.Icon; // https://en.wikipedia.org/wiki/Yakuza_Ishin
        public override int Version => 5;
        public override System.Text.Encoding FileEncoding => new Encoding();

        private GameFileContainer GetBootpar()
        {
            var tableSearch =
                new GameFileSearch
                {
                    RelativePath = ".",
                    SearchPattern =
                        "ability.bin;caption.bin;chase_base.bin;chase_condition.bin;complete_haruka.bin;complete_heat.bin;complete_shisho.bin;continuepoint.bin;dictionary.bin;dictionary_ignore.bin;environment.bin;explanation_main_scenario.bin;explanation_sub_story.bin;item.bin;item_alias.bin;item_mark.bin;item_weapon_parameter.bin;kiyaku.bin;otazunemono.bin;pet_bonus.bin;restaurant_menu.bin;tips_tutorial.bin",
                    
                    IsWildcard = true,
                    RecursiveSearch = false,
                    FileType = typeof(YakuzaGame.Files.Table.File)
                };

            var bootpar = new GameFileContainer
            {
                Path = @"data\bootpar\boot.par",
                Type = ContainerType.CompressedFile
            };

            bootpar.FileSearches.Add(tableSearch);
            return bootpar;
        }

        private GameFileContainer GetScenario()
        {
            var search =
                new GameFileSearch
                {
                    RelativePath = ".",
                    SearchPattern = "scenario2.bin",
                    IsWildcard = false,
                    RecursiveSearch = false,
                    FileType = typeof(Files.Scenario.File)
                };

            var container = new GameFileContainer
            {
                Path = @"data\scenario",
                Type = ContainerType.Folder
            };

            container.FileSearches.Add(search);
            return container;
        }

        private GameFileContainer GetStage()
        {
            var streetNameSearch =
                new GameFileSearch
                {
                    RelativePath = ".",
                    SearchPattern = "street_name.dat",
                    IsWildcard = true,
                    RecursiveSearch = false,
                    FileType = typeof(YakuzaGame.Files.StreetName.File)
                };

            var stage = new GameFileContainer
            {
                Path = @"data\stage\ps3\flag_data",
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
                    SearchPattern = "activity_list.bin;colosseum_participant.bin;commerce_*.bin;complete_haruka.bin;controller_explain.bin;correlation_*.bin;encount_loser_popup.bin;encounter_*.bin;explanation_soldier_mission.bin;farm_vegetable_list.bin;kyoukei_bird_param.bin;otazunemono.bin;pet_*.bin;response_chase.bin;response_roulette.bin;roulette_npc.bin;soldier_card_list.bin;soldier_leader_skill_list.bin;soldier_normal_skill_list.bin;soldier_training_mission.bin;tenkei_blog.bin;treasure_random_item.bin;tutorial.bin;ultimate.bin;virtue_shop_*.bin",
                    IsWildcard = true,
                    RecursiveSearch = false,
                    FileType = typeof(YakuzaGame.Files.Table.File)
                };

            var par = new GameFileContainer
            {
                Path = @"data\staypar\stay.par",
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
                    FileType = typeof(Files.AiPopup.File)
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
                Path = @"data\wdr_par\common.par",
                Type = ContainerType.CompressedFile
            };
            wdr_par_c_common.FileSearches.Add(aiPopupSearch);
            wdr_par_c_common.FileSearches.Add(common_blacksmithSearch);
            wdr_par_c_common.FileSearches.Add(common_presentSearch);
            wdr_par_c_common.FileSearches.Add(common_saleSearch);

            result.Add(wdr_par_c_common);

            return result;
        }

        private IList<GameFileContainer> GetWdr()
        {
            var result = new List<GameFileContainer>();

            var wdr_restaurantSearch =
                new GameFileSearch
                {
                    RelativePath = "shop",
                    SearchPattern = "restaurant????.bin;ex_shop????.bin;shop????.bin",
                    IsWildcard = true,
                    RecursiveSearch = false,
                    FileType = typeof(Files.Restaurant.File)
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

            var wdr_par = new GameFileContainer
            {
                Path = @"data\wdr_par\wdr.par",
                Type = ContainerType.CompressedFile
            };

            wdr_par.FileSearches.Add(wdr_restaurantSearch);
            wdr_par.FileSearches.Add(wdr_msgSearch);

            result.Add(wdr_par);

            return result;
        }

        public override GameFileContainer[] GetContainers(string path)
        {
            var result = new List<GameFileContainer>();

            result.Add(GetBootpar());
            result.Add(GetScenario());
            result.Add(GetStage());
            result.Add(GetStaypar());
            result.AddRange(GetWdrCommon());
            result.AddRange(GetWdr());

            result.Sort();
            return result.ToArray();
        }
    }
}

