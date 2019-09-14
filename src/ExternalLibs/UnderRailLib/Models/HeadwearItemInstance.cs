using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.TimelapseVertigo.Rules.Items;

namespace UnderRailLib.Models
{
    [EncodedTypeName("HII")]
    [Serializable]
    public sealed class HeadwearItemInstance : ArmorItemInstance, IItemInstance<HeadwearItem>
    {
        private HeadwearItemInstance(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
