using System;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.TimelapseVertigo.Rules.Capabilities
{
    [EncodedTypeName("eCC")]
    [Serializable]
    public enum CooldownCategory
    {
        None,
        Sprint,
        DirtyKick,
        Stealth,
        EvasiveManeuvers,
        TrapArming = 100,
        Bilocation = 201
    }
}
