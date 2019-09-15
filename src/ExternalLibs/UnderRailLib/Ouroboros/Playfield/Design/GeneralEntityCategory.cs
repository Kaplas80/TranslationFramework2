using System;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Ouroboros.Playfield.Design
{
    [EncodedTypeName("eGEC")]
    [Serializable]
    public enum GeneralEntityCategory
    {
        Terrain,
        Wall,
        Door,
        Object,
        Creature,
        Miscellaneous
    }
}
