using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.TimelapseVertigo.Rules.Items;

namespace UnderRailLib.Models
{
    [EncodedTypeName("NEII")]
    [Serializable]
    public class NonEquippableItemInstance : ItemInstance, IItemInstance<NonEquippableItem>
    {
        protected NonEquippableItemInstance(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
