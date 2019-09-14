using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("BI")]
    [Serializable]
    public sealed class FootwearItem : ArmorItem
    {
        private FootwearItem(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
