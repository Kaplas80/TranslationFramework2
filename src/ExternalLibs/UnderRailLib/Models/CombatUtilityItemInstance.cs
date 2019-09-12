using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.TimelapseVertigo.Rules.Items;

namespace UnderRailLib.Models
{
    [EncodedTypeName("QSII")]
    [Serializable]
    public class CombatUtilityItemInstance : ItemInstance, IItemInstance<CombatUtilityItem>, ISerializable
    {
        protected CombatUtilityItemInstance(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
