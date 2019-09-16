using System.Collections.Generic;
using System.IO;

namespace UnderRailTool
{
    public class AssemblyResolver : aiu
    {
        public AssemblyResolver() : base(113)
        {
        }

        protected override Dictionary<string, SearchOption> m9()
        {
            var dictionary = new Dictionary<string, SearchOption>();
            var ddi = c0t.e();
            if (ddi != null)
            {
                dictionary.Add(ddi.v6(), SearchOption.TopDirectoryOnly);
            }
            return dictionary;
        }
    }
}
