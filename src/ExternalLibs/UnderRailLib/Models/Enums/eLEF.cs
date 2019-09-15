using System;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models.Enums
{
    [EncodedTypeName("eLEF")]
    [Flags]
    [Serializable]
    public enum eLEF
    {
        a = 1,
        b = 2,
        c = 4,
        d = 8,
        e = 16,
        f = 32,
        g = 64,
        h = 128,
        i = 256
    }
}
