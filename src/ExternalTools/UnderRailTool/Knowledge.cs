using System.Collections.Generic;

namespace UnderRailTool
{
    public static class Knowledge
    {
        // "KI" class = aj8
        public static Dictionary<string, string> GetSubtitles(string gameFile)
        {
            var model = FileManager.Load<aj8>(gameFile, true);

            return model.a;
        }

        public static aj8 SetSubtitles(string gameFile, Dictionary<string, string> texts)
        {
            var model = FileManager.Load<aj8>(gameFile, true);

            model.a = texts;

            return model;
        }
    }
}
