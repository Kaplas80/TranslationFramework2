using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("XPBLNC")]
    [Serializable]
    public sealed class XPBLNC : Job
    {
        private XPBLNC(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
