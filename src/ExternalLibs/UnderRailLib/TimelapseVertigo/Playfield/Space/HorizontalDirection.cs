using System;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.TimelapseVertigo.Playfield.Space
{
    [EncodedTypeName("eHDIR")]
    [Serializable]
    public enum HorizontalDirection
    {
        North,
        Northeast,
        East,
        Southeast,
        South,
        Southwest,
        West,
        Northwest
    }
}
