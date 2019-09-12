using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("FOGMSSJ")]
    [Serializable]
    public sealed class FOGMSSJ : Job
    {
        private FOGMSSJ(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
