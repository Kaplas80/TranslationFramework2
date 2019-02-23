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

        public override GameFileContainer[] GetContainers(string path)
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

            //result.AddRange(auth_w64_containers.GetContainers(path));

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
            bootpar.FileSearches.Add(ddsSearch);

            result.Add(bootpar);

            var imbSearch =
                new GameFileSearch
                {
                    RelativePath = ".",
                    SearchPattern =
                        "uid0419000a.imb;uid0419000b.imb;uid0419000c.imb;uid0419000d.imb;uid0419000e.imb;uid04190010.imb;uid04190011.imb;uid04190012.imb;uid04190013.imb;uid04190014.imb;uid04190015.imb;uid04190016.imb;uid04190017.imb;uid04190018.imb;uid04190019.imb;uid0419001a.imb;uid0419001b.imb;uid0419001c.imb;uid0419001d.imb;uid0419001e.imb;uid0419001f.imb;uid04190029.imb;uid0419002a.imb;uid0419002b.imb;uid0419002c.imb;uid0419002d.imb;uid0419002e.imb;uid0419002f.imb;uid04190034.imb;uid04190035.imb;uid04190036.imb;uid04190037.imb;uid04190038.imb;uid043314ad.imb;uid043314ee.imb;uid043314ef.imb;uid043314f0.imb;uid043314f1.imb;uid043315d1.imb;uid04331c1c.imb;uid04331c1d.imb;uid04331c1e.imb;uid04331c1f.imb;uid04331c20.imb;uid04440233.imb;uid04440234.imb;uid04440235.imb;uid04440236.imb;uid04440237.imb;uid04440238.imb;uid04440239.imb;uid0444023b.imb;uid0444023d.imb;uid0444023e.imb;uid0444023f.imb;uid04440240.imb;uid04440241.imb;uid04440242.imb;uid04440244.imb;uid04440248.imb;uid0444024a.imb;uid0444024c.imb;uid0444024d.imb;uid0444024e.imb;uid0444024f.imb;uid04440250.imb;uid04440252.imb;uid04440254.imb;uid04440255.imb;uid04440284.imb;uid044402c7.imb;uid044402c8.imb;uid044402c9.imb;uid044402ca.imb;uid044402cb.imb;uid044402cc.imb;uid044402cd.imb;uid044402cf.imb;uid044402d0.imb;uid044402d1.imb;uid044402d2.imb;uid044402d3.imb;uid044402d4.imb;uid044402d5.imb;uid044402d6.imb;uid044402d7.imb;uid044402d8.imb;uid044402d9.imb;uid044402da.imb;uid044402db.imb;uid044402dc.imb;uid044402de.imb;uid044402df.imb;uid044402e0.imb;uid044402e1.imb;uid044402e2.imb;uid044402e3.imb;uid044402e4.imb;uid044402e5.imb;uid044402e6.imb;uid044402e7.imb;uid044402e8.imb;uid044402e9.imb;uid044402ea.imb;uid044402eb.imb",
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
                Path = @"data\wdr_par_c\common.par", Type = ContainerType.CompressedFile
            };
            wdr_par_c_common.FileSearches.Add(aiPopupSearch);
            wdr_par_c_common.FileSearches.Add(common_armsRepairSearch);
            wdr_par_c_common.FileSearches.Add(common_blacksmithSearch);
            wdr_par_c_common.FileSearches.Add(common_presentSearch);
            wdr_par_c_common.FileSearches.Add(common_saleSearch);

            result.Add(wdr_par_c_common);

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

            result.Add(wdr_par);

            result.Sort();
            return result.ToArray();
        }
    }
}

