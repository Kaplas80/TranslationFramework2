using System.Collections.Generic;
using System.IO;
using System.Linq;
using TF.Core.Helpers;

namespace TF.Core.Entities
{
    public class GameFileContainerSearch
    {
        public string RelativePath { get; set; }
        public string SearchPattern { get; set; }
        public bool RecursiveSearch { get; set; }
        public ContainerType TypeSearch { get; set; }
        public IList<GameFileSearch> FileSearches { get; }
        public IList<string> Exclusions { get; }

        public GameFileContainerSearch()
        {
            FileSearches = new List<GameFileSearch>();
            Exclusions = new List<string>();
        }

        public IList<GameFileContainer> GetContainers(string path)
        {
            var fullPath = Path.Combine(path, RelativePath);

            string[] searchResult;
            if (TypeSearch == ContainerType.Folder)
            {
                searchResult = Directory.GetDirectories(fullPath, SearchPattern,
                    RecursiveSearch ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
            }
            else
            {
                searchResult = Directory.GetFiles(fullPath, SearchPattern,
                    RecursiveSearch ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
            }

            var result = new List<GameFileContainer>();
            foreach (var f in searchResult)
            {
                var excluded = Exclusions.Any(exclusion => f.Contains(exclusion));

                if (excluded) continue;

                var container = new GameFileContainer
                {
                    Path = PathHelper.GetRelativePath(path, f),
                    Type = TypeSearch,
                    FileSearches = FileSearches
                };

                result.Add(container);
            }

            return result;
        }
    }
}