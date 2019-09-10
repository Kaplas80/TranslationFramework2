using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("SCHJ")]
    [Serializable]
    public sealed class SCHJ : Job
    {
        private SCHJ(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
