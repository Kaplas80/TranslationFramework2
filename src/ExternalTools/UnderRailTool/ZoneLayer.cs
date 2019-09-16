using System.Collections.Generic;

namespace UnderRailTool
{
    public static class ZoneLayer
    {
        public static Dictionary<string, string> GetSubtitles(string gameFile)
        {
            var model = FileManager.Load<cx0>(gameFile, true);

            var result = new Dictionary<string, string>();

            foreach (var area in model.e())
            {
                foreach (var entity in area.Entities)
                {
                    var id = entity.n2().ToString();
                    var name = entity.nb();
                    if (!string.IsNullOrEmpty(name))
                    {
                        result.Add(id, name);
                    }
                }
            }

            return result;
        }

        public static cx0 SetSubtitles(string gameFile, Dictionary<string, string> texts)
        {
            var model = FileManager.Load<cx0>(gameFile, true);

            foreach (var area in model.e())
            {
                foreach (var entity in area.Entities)
                {
                    var id = entity.n2().ToString();
                    if (texts.TryGetValue(id, out var name))
                    {
                        entity.q(name);
                    }
                }
            }

            return model;
        }
    }
}
