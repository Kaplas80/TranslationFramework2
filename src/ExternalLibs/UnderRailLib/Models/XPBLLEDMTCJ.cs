using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("XPBLLEDMTCJ")]
    [Serializable]
    public sealed class XPBLLEDMTCJ : Job
    {
        private XPBLLEDMTCJ(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
