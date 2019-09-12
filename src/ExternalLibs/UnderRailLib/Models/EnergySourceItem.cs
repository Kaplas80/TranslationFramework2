using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("ESI2")]
    [Serializable]
    public sealed class EnergySourceItem : NonEquippableItem
    {
        private EnergySourceItem(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
