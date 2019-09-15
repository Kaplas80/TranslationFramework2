using System;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models.Enums
{
    [EncodedTypeName("eART")]
    [Flags]
    [Serializable]
    public enum eART
    {
        a = 0,
        b = 1,
        c = 2,
        d = 4,
        e = 8,
        f = 16,
        g = 32
    }
}
