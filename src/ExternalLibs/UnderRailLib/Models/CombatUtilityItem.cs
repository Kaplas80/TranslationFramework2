using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("QSI")]
    [Serializable]
    public class CombatUtilityItem : Item
    {
        protected CombatUtilityItem(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
