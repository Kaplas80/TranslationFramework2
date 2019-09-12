using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("GSFWR2")]
    [Serializable]
    public sealed class GSFWR2 : Job
    {
        private GSFWR2(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
