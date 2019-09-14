using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("XPBLLEHCSJ")]
    [Serializable]
    public sealed class XPBLLEHCSJ : Job
    {
        private XPBLLEHCSJ(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
