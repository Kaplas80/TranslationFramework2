using System.Collections.Generic;

namespace UnderRailTool
{
    public static class Knowledge
    {
        // "KI" class = anm
        public static Dictionary<string, string> GetSubtitles(string gameFile)
        {
            var model = FileManager.Load<anm>(gameFile, true);

            var result = new Dictionary<string, string>();
            foreach (var kvp in model.a)
            {
                result.Add(kvp.Key, kvp.Key);
                result.Add($"{kvp.Key}_value", kvp.Value);
            }

            return result;
        }

        public static anm SetSubtitles(string gameFile, Dictionary<string, string> texts)
        {
            var model = FileManager.Load<anm>(gameFile, true);

            var rebuilt = new Dictionary<string, string>();
            foreach (var kvp in texts)
            {
                if (!kvp.Key.EndsWith("_value"))
                {
                    var value = texts[$"{kvp.Key}_value"];
                    rebuilt.Add(kvp.Value, value);
                }
            }

            model.a = rebuilt;

            return model;
        }
    }
}
