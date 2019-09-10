using System;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.TimelapseVertigo.Rules.Items
{
    [EncodedTypeName("eRC")]
    [Serializable]
    public enum ItemRepairCategory
    {
        Mechanical,
        Fabric,
        Electronic
    }
}
