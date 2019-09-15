using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.TimelapseVertigo.Rules.Items;

namespace UnderRailLib.Models
{
    [EncodedTypeName("BII")]
    [Serializable]
    public sealed class FootwearItemInstance : EquippableItemInstance, IItemInstance<FootwearItem>
    {
        private FootwearItemInstance(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
