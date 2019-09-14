using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("XPBLAASITZ")]
    [Serializable]
    public sealed class XPBLAASITZ : Job
    {
        private XPBLAASITZ(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
