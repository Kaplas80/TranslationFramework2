using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("GI")]
    [Serializable]
    public class GrenadeItem : CombatUtilityItem
    {
        protected GrenadeItem(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
