using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("LUDFDTHJ")]
    [Serializable]
    public sealed class LUDFDTHJ : Job
    {
        private LUDFDTHJ(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
