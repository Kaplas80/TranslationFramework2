using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("XPBLLEUCJ")]
    [Serializable]
    public sealed class XPBLLEUCJ : Job
    {
        private XPBLLEUCJ(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
