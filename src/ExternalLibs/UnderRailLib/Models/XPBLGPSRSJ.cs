using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("XPBLGPSRSJ")]
    [Serializable]
    public sealed class XPBLGPSRSJ : Job
    {
        private XPBLGPSRSJ(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
