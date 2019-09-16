using System.Collections.Generic;

namespace UnderRailTool
{
    public static class Items
    {
        public static Dictionary<string, string> GetSubtitles(string gameFile)
        {
            var model = FileManager.Load<dpp>(gameFile, true);

            var name = model.a2();
            var description = model.aw();

            var result = new Dictionary<string, string>(2);
            if (!string.IsNullOrEmpty(name))
            {
                result.Add("Name", name);
            }

            if (!string.IsNullOrEmpty(description))
            {
                result.Add("Description", description);
            }

            return result;
        }

        public static dpp SetSubtitles(string gameFile, Dictionary<string, string> texts)
        {
            var model = FileManager.Load<dpp>(gameFile, true);

            if (texts.TryGetValue("Name", out var name))
            {
                model.j(name);
            }

            if (texts.TryGetValue("Description", out var description))
            {
                model.h(description);
            }

            return model;
        }
    }
}
