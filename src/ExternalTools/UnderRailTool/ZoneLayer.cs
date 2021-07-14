using System.Collections.Generic;

namespace UnderRailTool
{
    public static class ZoneLayer
    {
        // "ZL" class = da3
        public static Dictionary<string, string> GetSubtitles(string gameFile)
        {
            var model = FileManager.Load<da3>(gameFile, true);

            var result = new Dictionary<string, string>();

            foreach (ce5 area in model.e()) // _AreaLayers
            {
                foreach (br0 entity in area.Entities)
                {
                    string id = entity.o6().ToString(); // base get "I"
                    string name = entity.w5(); // base get "N"
                    if (!string.IsNullOrEmpty(name))
                    {
                        result.Add(id, name);
                    }
                }
            }

            return result;
        }

        public static da3 SetSubtitles(string gameFile, Dictionary<string, string> texts)
        {
            var model = FileManager.Load<da3>(gameFile, true);

            foreach (ce5 area in model.e())
            {
                foreach (br0 entity in area.Entities)
                {
                    string id = entity.o6().ToString();
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
