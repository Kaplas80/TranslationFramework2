using System.Collections.Generic;
using System.Drawing;
using System.IO;
using TF.Core;
using TF.Core.Entities;
using TF.Core.Files.DDS;
using TFGame.Yakuza0.Files;

namespace TFGame.Yakuza0
{
    public class Game : IGame
    {
        public string Id => "193b8191-39e2-4ad7-b0bf-9cb413bb910f";
        public string Name => "Yakuza 0";
        public string Description => "";
        public Image Icon => Resources.Icon;
        public int Version => 1;

        public GameFileContainer[] Containers 
        {
            get
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

                var auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a01_010.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                auth_w64_e.FileSearches.Add(ddsSearch);
                result.Add(auth_w64_e);

                /*auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a01_020.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a01_030.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a01_050.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a01_055.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a01_080.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a01_100.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a01_110.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a01_120.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a02_010.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a02_020.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a02_030.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a03_010.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a03_020.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a03_030.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a03_050.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a03_060.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a03_070.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a04_005.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a04_010.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a04_020.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a04_030.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a04_040.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a04_050.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a04_060.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a05_010.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a05_020.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a06_020.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a06_030.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a06_050.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a06_060.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a06_070.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a06_080.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a06_090.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a07_010.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a07_020.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a07_030.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a07_040.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a07_050.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a08_010.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a08_020.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a08_030.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a09_010.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a09_020.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a09_030.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a10_010.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a10_020.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a10_030.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a11_010.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a11_015.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a11_020.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a11_030.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a11_040.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a11_050.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a12_010.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a12_020.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a12_025.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a12_040.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a13_010.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a13_020.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a13_025.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a13_030.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a13_040.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a14_010.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a14_020.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a14_030.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a14_040.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a14_050.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a14_060.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a15_010.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a15_020.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a15_030.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a15_040.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a15_050.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a15_060.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a16_010.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a16_020.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a16_030.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a16_040.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a16_050.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a16_060.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);

                result.Add(auth_w64_e);
                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a16_070.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a17_010.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a17_020.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a17_030.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a17_040.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a17_050.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a17_060.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a17_070.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a17_090.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a17_100.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a17_110.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a17_120.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a17_130.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a17_140.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a17_150.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a17_160.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a17_170.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a17_180.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a17_190.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\a17_200.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\f01_010.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\f01_020.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\f01_030.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\f01_040.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\f04_010.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\f04_010_.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\f10_010.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\f13_010.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\f13_020.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\f16_010.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\i00_000.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\p05_000.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\p07_000.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\p09_000.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\p11_000.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\p13_000.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\p15_000.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\p17_000.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                auth_w64_e = new GameFileContainer
                    {Path = "data\\auth_w64_e\\s00_000.par", Type = ContainerType.CompressedFile};
                auth_w64_e.FileSearches.Add(cmnSearch);
                result.Add(auth_w64_e);

                var hact = new GameFileContainer
                    { Path = "data\\hact.par", Type = ContainerType.CompressedFile };
                hact.FileSearches.Add(cmnSearch);
                result.Add(hact);*/

                result.Sort();
                return result.ToArray();
            }
        }

        public TranslationFile GetFile(string path, string changesFolder)
        {
            TranslationFile result;

            var fileName = Path.GetFileName(path);
            var extension = Path.GetExtension(path);

            if (fileName.EndsWith("cmn.bin"))
            {
                result = new CmnBinFile(path, changesFolder);
            }
            else
            {
                switch (extension)
                {
                    case ".dds":
                    {
                        result = new TF.Core.Files.DDS.DDSFile(path, changesFolder);
                        break;
                    }

                    default:
                    {
                        result = new TranslationFile(path, changesFolder);
                        break;
                    }
                }
            }
            

            return result;
        }

        public void ExtractFile(string inputFile, string outputPath)
        {
            var extension = Path.GetExtension(inputFile);

            if (extension == ".par")
            {
                var parFile = new ParFile(inputFile);
                parFile.Extract(outputPath);
            }
        }

        public void RepackFile(string inputPath, string outputFile, bool compress)
        {
            var extension = Path.GetExtension(outputFile);

            if (extension == ".par")
            {
                ParFile.Repack(inputPath, outputFile, compress);
            }
        }
    }
}
