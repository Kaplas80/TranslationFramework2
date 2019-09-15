using System;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.TimelapseVertigo.Rules.Effects
{
    [EncodedTypeName("eSTES")]
    [Serializable]
    public enum StatusEffectState
    {
        Uninitialized,
        Executing,
        Expired
    }
}
