using System;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models.Enums
{
    [EncodedTypeName("ePAC")]
    [Flags]
    [Serializable]
    public enum ePAC : long
    {
        a = 0L,
        b = 1L,
        c = 2L
    }
}
