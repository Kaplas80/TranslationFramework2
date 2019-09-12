using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.TimelapseVertigo.Rules.Items;

namespace UnderRailLib.Models
{
    [EncodedTypeName("ASII")]
    [Serializable]
    public sealed class ArmorSuitItemInstance : ArmorItemInstance, IItemInstance<ArmorSuitItem>, ISerializable
    {
        private ArmorSuitItemInstance(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
