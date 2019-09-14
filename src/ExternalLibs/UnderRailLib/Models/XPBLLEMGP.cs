using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("XPBLLEMGP")]
    [Serializable]
    public sealed class XPBLLEMGP : Job
    {
        private XPBLLEMGP(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
