using System.Collections.Generic;
using System.IO;

namespace UnderRailTool
{
    // Search for "Initializing assembly resolver"
    // I have to make this class because the one in Underrail.exe is internal
    public class AssemblyResolver : ajl
    {
        public AssemblyResolver() : base(113)
        {
        }

        protected override Dictionary<string, SearchOption> anq()
        {
            var dictionary = new Dictionary<string, SearchOption>();
            dex dex = c1y.e();
            if (dex != null)
            {
                dictionary.Add(dex.vw(), SearchOption.TopDirectoryOnly);
            }
            return dictionary;
        }
    }
}
