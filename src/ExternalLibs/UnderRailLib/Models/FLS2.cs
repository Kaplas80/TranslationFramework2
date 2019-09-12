using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("FLS2")]
    [Serializable]
    public sealed class FLS2 : Job
    {
        private FLS2(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
