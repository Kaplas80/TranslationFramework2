using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("XPBLLEGMPCTS")]
    [Serializable]
    public sealed class XPBLLEGMPCTS : Job
    {
        private XPBLLEGMPCTS(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
