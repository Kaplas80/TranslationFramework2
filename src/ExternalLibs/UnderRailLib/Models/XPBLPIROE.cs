using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("XPBLPIROE")]
    [Serializable]
    public sealed class XPBLPIROE : Job
    {
        private XPBLPIROE(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
