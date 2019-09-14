using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("BI1")]
    [Serializable]
    public sealed class BeltItem : EquippableItem
    {
        private BeltItem(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
