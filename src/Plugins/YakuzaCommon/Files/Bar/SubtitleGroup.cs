using System.Collections.Generic;

namespace YakuzaCommon.Files.Bar
{
    internal class SubtitleGroup
    {
        public int[] Data;
        public List<SimpleSubtitle.Subtitle> Subtitles;

        public SubtitleGroup()
        {
            Data = new int[22];
            Subtitles = new List<SimpleSubtitle.Subtitle>();
        }
    }
}
