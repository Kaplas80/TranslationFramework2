using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("HI")]
    [Serializable]
    public sealed class HeadwearItem : ArmorItem
    {
        private HeadwearItem(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
