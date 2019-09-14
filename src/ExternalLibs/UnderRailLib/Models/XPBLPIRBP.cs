using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("XPBLPIRBP")]
    [Serializable]
    public sealed class XPBLPIRBP : Job
    {
        private XPBLPIRBP(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
