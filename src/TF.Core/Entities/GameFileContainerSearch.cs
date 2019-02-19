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
        public IList<GameFileSearch> FileSearches { get; set; }

        public GameFileContainerSearch()
        {
            FileSearches = new List<GameFileSearch>();
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

            return searchResult.Select(f => new GameFileContainer {Path = PathHelper.GetRelativePath(path, f), Type = TypeSearch, FileSearches = FileSearches}).ToList();
        }
    }
}