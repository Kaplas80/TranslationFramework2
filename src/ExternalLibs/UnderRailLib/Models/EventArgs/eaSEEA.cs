using System;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models.EventArgs
{
    [EncodedTypeName("eaSEEA")]
    [Serializable]
    public class eaSEEA : System.EventArgs
    {
        public SE1 ObjectSE1 { get; }
        
        public EffectContext EffectContext { get; }

        public eaSEEA(SE1 objectSE1, EffectContext effectContext)
        {
            ObjectSE1 = objectSE1;
            EffectContext = effectContext;
        }

    }
}
