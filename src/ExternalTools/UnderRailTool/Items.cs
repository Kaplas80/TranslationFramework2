using System.Collections.Generic;

namespace UnderRailTool
{
    public static class Items
    {
        // "I" class = bye
        public static Dictionary<string, string> GetSubtitles(string gameFile)
        {
            var model = FileManager.Load<bye>(gameFile, true);

            string name = model.ax();
            string description = model.ay();

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

        public static bye SetSubtitles(string gameFile, Dictionary<string, string> texts)
        {
            var model = FileManager.Load<bye>(gameFile, true);

            if (texts.TryGetValue("Name", out string name))
            {
                model.f(name);
            }

            if (texts.TryGetValue("Description", out string description))
            {
                model.h(description);
            }

            return model;
        }
    }
}
