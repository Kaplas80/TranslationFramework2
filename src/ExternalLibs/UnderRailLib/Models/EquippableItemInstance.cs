using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.TimelapseVertigo.Rules.Items;

namespace UnderRailLib.Models
{
    [EncodedTypeName("EII")]
    [Serializable]
    public abstract class EquippableItemInstance : ItemInstance, IItemInstance<EquippableItem>, ISerializable
    {
        protected EquippableItemInstance(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
