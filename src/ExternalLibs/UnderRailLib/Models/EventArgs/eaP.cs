using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models.EventArgs
{
    [EncodedTypeName("eaP")]
    public sealed class eaP : System.EventArgs
    {
        public float Float { get; }

        public eaP(float f)
        {
            Float = f;
        }
    }
}
