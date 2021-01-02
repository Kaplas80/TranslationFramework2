using System.Collections.Generic;

namespace UnderRailTool
{
    public static class ZoneLayer
    {
        // "ZL" class = cy3
        public static Dictionary<string, string> GetSubtitles(string gameFile)
        {
            var model = FileManager.Load<cy3>(gameFile, true);

            var result = new Dictionary<string, string>();

            foreach (b5t area in model.e()) // _AreaLayers
            {
                foreach (bko entity in area.Entities)
                {
                    string id = entity.n6().ToString();
                    string name = entity.nj();
                    if (!string.IsNullOrEmpty(name))
                    {
                        result.Add(id, name);
                    }
                }
            }

            return result;
        }

        public static cy3 SetSubtitles(string gameFile, Dictionary<string, string> texts)
        {
            var model = FileManager.Load<cy3>(gameFile, true);

            foreach (b5t area in model.e())
            {
                foreach (bko entity in area.Entities)
                {
                    string id = entity.n6().ToString();
                    if (texts.TryGetValue(id, out string name))
                    {
                        entity.q(name);
                    }
                }
            }

            return model;
        }
    }
}
