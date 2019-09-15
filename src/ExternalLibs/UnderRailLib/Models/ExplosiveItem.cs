using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("EXI")]
    [Serializable]
    public sealed class ExplosiveItem : NonEquippableItem
    {
        private ExplosiveItem(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
