using System.Collections.Generic;

namespace UnderRailTool
{
    public static class Knowledge
    {
        public static Dictionary<string, string> GetSubtitles(string gameFile)
        {
            var model = FileManager.Load<aji>(gameFile, true);

            return model.a;
        }

        public static aji SetSubtitles(string gameFile, Dictionary<string, string> texts)
        {
            var model = FileManager.Load<aji>(gameFile, true);

            model.a = texts;

            return model;
        }
    }
}
