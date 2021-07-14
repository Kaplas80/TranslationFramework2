using System.Collections.Generic;
using System.IO;

namespace UnderRailTool
{
    // Search for "Initializing assembly resolver"
    // I have to make this class because the one in Underrail.exe is internal
    public class AssemblyResolver : amu
    {
        public AssemblyResolver() : base(113)
        {
        }

        protected override Dictionary<string, SearchOption> aoi()
        {
            var dictionary = new Dictionary<string, SearchOption>();
            dsd dsd = ded.d();
            if (dsd != null)
            {
                dictionary.Add(dsd.km(), SearchOption.TopDirectoryOnly);
            }
            return dictionary;
        }
    }
}
