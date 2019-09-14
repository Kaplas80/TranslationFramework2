using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("XPBLPIRGRSJ")]
    [Serializable]
    public sealed class XPBLPIRGRSJ : Job
    {
        private XPBLPIRGRSJ(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
