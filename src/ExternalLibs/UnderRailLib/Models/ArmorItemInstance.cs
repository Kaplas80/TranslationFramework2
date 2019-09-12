using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.TimelapseVertigo.Rules.Items;

namespace UnderRailLib.Models
{
    [EncodedTypeName("AII")]
    [Serializable]
    public abstract class ArmorItemInstance : EquippableItemInstance, IItemInstance<ArmorItem>, ISerializable
    {
        protected ArmorItemInstance(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
