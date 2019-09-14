using System;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models.Enums
{
    [EncodedTypeName("eCAT")]
    [Flags]
    [Serializable]
    public enum eCAT : long
    {
        a = 0L,
        b = 1L,
        c = 2L,
        d = 4L,
        e = 8L,
        f = 16L,
        g = 32L,
        h = 64L,
        i = 128L,
        j = 256L,
        k = 512L,
        l = 1024L,
        m = 2048L,
        n = 4096L,
        o = 8192L,
        p = 16384L,
        q = 32768L,
        r = 65536L
    }

}
