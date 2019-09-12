using System;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.TimelapseVertigo.Rules.Combat
{
    [EncodedTypeName("eDMT")]
    [Serializable]
    public enum DamageType
    {
        Mechanical,
        Heat,
        Cold,
        Electricity,
        Acid,
        Energy,
        Bio = 7
    }
}
