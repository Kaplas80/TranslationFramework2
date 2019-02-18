using System.Collections.Generic;
using System.IO;

namespace TF.Core.Entities
{
    public class GameFileSearch
    {
        public string RelativePath { get; set; }
        public string SearchPattern { get; set; }
        public bool IsWildcard { get; set; }
        public bool RecursiveSearch { get; set; }

        public string[] GetFiles(string path)
        {
            var result = new List<string>();

            var fullPath = Path.Combine(path, RelativePath);
            if (IsWildcard)
            {
                var files = Directory.GetFiles(fullPath, SearchPattern,
                    RecursiveSearch ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);

                result.AddRange(files);
            }
            else
            {
                if (File.Exists(Path.Combine(fullPath, SearchPattern)))
                {
                    result.Add(Path.Combine(fullPath, SearchPattern));
                }
            }

            return result.ToArray();
        }
    }
}