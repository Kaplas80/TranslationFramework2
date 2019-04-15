using System.Collections.Generic;
using System.Drawing;
using System.Text;
using TF.Core.Entities;

namespace TFGame.ShiningResonance
{
    public class Game : CRIWareGame.CRIWareGame
    {
        public override string Id => "f19142d4-d6bb-4fd1-bcfe-1cb29868df7b";
        public override string Name => "Shining Resonance Refrain";
        public override string Description => "Versión en inglés Steam (10/09/2018)";
        public override Image Icon => Resources.Icon; // https://www.deviantart.com/clarence1996/art/Shining-Resonance-Refrain-754760674
        public override int Version => 1;
        public override Encoding FileEncoding => Encoding.UTF8;

        public override GameFileContainer[] GetContainers(string path)
        {
            var result = new List<GameFileContainer>();

            var fontFiles = new GameFileSearch
            {
                RelativePath = @"data\resource\font",
                SearchPattern = "*.*",
                IsWildcard = true,
                RecursiveSearch = false,
                FileType = typeof(Files.FONT.File)
            };

            var drg000 = new GameFileContainer
            {
                Path = @".\drg000.cpk",
                Type = ContainerType.CompressedFile
            };

            drg000.FileSearches.Add(fontFiles);

            var mtpFiles = new GameFileSearch
            {
                RelativePath = @"data\resource\mtpa_en",
                SearchPattern = "*.mtp",
                IsWildcard = true,
                RecursiveSearch = false,
                FileType = typeof(Files.Mtp.File),
            };

            var htxFiles = new GameFileSearch
            {
                RelativePath = @"data\resource\interface_en\htx",
                SearchPattern = "chapter_??.htx;sdif_battle_counter.htx;sdif_battle_encount.htx;sdif_battle_fromarge.htx;sdif_battle_gauge_01.htx;sdif_battle_hud_fonts.htx;sdif_battle_imgfont.htx;sdif_battle_parts.htx;sdif_camp_band_fonts.htx;sdif_camp_chara128x1024_c00.htx;sdif_camp_molds.htx;sdif_camp_option.htx;sdif_camp_option_02.htx;sdif_camp_option_03.htx;sdif_camp_option_select_01.htx;sdif_camp_party.htx;sdif_camp_titles.htx;sdif_com_windows.htx;sdif_escape_frame.htx;sdif_event_icon.htx;sdif_evn_*.htx;sdif_field_imgfont02.htx;sdif_field_result_02.htx;sdif_gallery_texture01.htx;sdif_gameover_contents.htx;sdif_grimoire_cutin.htx;sdif_grimoire_cutin02.htx;sdif_grimoire_cutin03.htx;sdif_grimoire_titles.htx;sdif_kizuna_diagram.htx;sdif_loadimg_*.htx;sdif_mapname_omap??.htx;sdif_mapname_tmap01.htx;sdif_mov_ev*.htx;sdif_nev*.htx;sdif_quest_cutin.htx;sdif_shop_fonts_02.htx;sdif_shop_fonts_a.htx;sdif_shop_fonts_b.htx;sdif_shop_fonts_c.htx;sdif_shop_fonts_d.htx;sdif_shop_frame.htx;sdif_skill_tuiningfont.htx;sdif_skill_tuningline.htx;sdif_song_subtitle.htx;sdif_timecounter01.htx;sdif_title_imgfonts.htx;sdif_title_mode.htx;sdif_title_parts01.htx",
                IsWildcard = true,
                RecursiveSearch = false,
                FileType = typeof(Files.Htx.File),
            };

            var drg022 = new GameFileContainer
            {
                Path = @".\drg022.cpk",
                Type = ContainerType.CompressedFile
            };

            drg022.FileSearches.Add(mtpFiles);
            drg022.FileSearches.Add(htxFiles);

            result.Add(drg000);
            result.Add(drg022);
            return result.ToArray();
        }
    }
}
