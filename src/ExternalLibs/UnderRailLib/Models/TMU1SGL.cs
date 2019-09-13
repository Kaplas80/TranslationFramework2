using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("TMU1SGL")]
    [Serializable]
    public sealed class TMU1SGL : Job
    {
        private TMU1SGL(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
