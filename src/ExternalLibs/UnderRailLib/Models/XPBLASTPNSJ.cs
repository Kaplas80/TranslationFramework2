using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("XPBLASTPNSJ")]
    [Serializable]
    public sealed class XPBLASTPNSJ : Job
    {
        private XPBLASTPNSJ(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
