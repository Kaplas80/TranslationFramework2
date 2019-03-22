using System.Collections.Generic;
using TF.Core.TranslationEntities;

namespace TFGame.TrailsSky.Files.DT
{
    public class DTElement 
    {
        public ushort Id { get; set; }
        public byte[] Unknown { get; set; }
        public IList<Subtitle> Strings { get; set; }

        public DTElement(int stringCount)
        {
            Strings = new List<Subtitle>(stringCount);
        }
    }
}
