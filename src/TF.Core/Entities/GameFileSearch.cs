using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TF.Core.Entities
{
    public class GameFileSearch
    {
        public string RelativePath { get; set; }
        public string SearchPattern { get; set; }
        public bool IsWildcard { get; set; }
        public bool RecursiveSearch { get; set; }
        public IList<string> Exclusions { get; }
        public Type FileType { get; set; }

        public GameFileSearch()
        {
            Exclusions = new List<string>();
        }

        public string[] GetFiles(string path)
        {
            var result = new List<string>();

            var fullPath = Path.GetFullPath(Path.Combine(path, RelativePath));
            if (IsWildcard)
            {
                var split = SearchPattern.Split(';');

                foreach (var s in split)
                {
                    var files = Directory.GetFiles(fullPath, s,
                        RecursiveSearch ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);

                    foreach (var file in files)
                    {
                        var excluded = Exclusions.Any(x => file.Contains(x));
                        if (!excluded)
                        {
                            result.Add(file);
                        }
                    }
                }
            }
            else
            {
                var files = Directory.GetFiles(fullPath, SearchPattern,
                    RecursiveSearch ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);

                foreach (var file in files)
                {
                    var excluded = Exclusions.Any(x => file.Contains(x));
                    if (!excluded)
                    {
                        result.Add(file);
                    }
                }
            }

            return result.ToArray();
        }
    }
}