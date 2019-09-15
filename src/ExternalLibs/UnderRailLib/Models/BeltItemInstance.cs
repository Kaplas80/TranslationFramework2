using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.TimelapseVertigo.Rules.Items;

namespace UnderRailLib.Models
{
    [EncodedTypeName("BII1")]
    [Serializable]
    public sealed class BeltItemInstance : EquippableItemInstance, IItemInstance<BeltItem>
    {
        private BeltItemInstance(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
