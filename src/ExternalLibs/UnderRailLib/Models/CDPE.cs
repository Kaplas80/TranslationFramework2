using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("CDPE")]
    [Serializable]
    public sealed class CDPE : StatusEffect
    {
        private CDPE(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
