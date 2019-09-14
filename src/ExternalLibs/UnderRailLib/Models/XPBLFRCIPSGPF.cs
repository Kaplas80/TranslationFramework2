using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("XPBLFRCIPSGPF")]
    [Serializable]
    public sealed class XPBLFRCIPSGPF : Job
    {
        private XPBLFRCIPSGPF(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
