using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("DCRTC")]
    [Serializable]
    public sealed class DCRTC : Job
    {
        private DCRTC(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
