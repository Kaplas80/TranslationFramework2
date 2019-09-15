using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models.EventArgs
{
    [EncodedTypeName("eaCSREA")]
    public sealed class eaCSREA : System.EventArgs
    {
        public int Int { get; }
        
        public bool Bool { get; }
        
        public eaCSREA(int i, bool b)
        {
            Int = i;
            Bool = b;
        }
    }
}
