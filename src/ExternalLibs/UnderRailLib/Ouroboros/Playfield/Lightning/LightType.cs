using System;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Ouroboros.Playfield.Lightning
{
    [EncodedTypeName("eLIT")]
    [Serializable]
    public enum LightType
    {
        Absolute,
        Increment,
        Decrement
    }
}
