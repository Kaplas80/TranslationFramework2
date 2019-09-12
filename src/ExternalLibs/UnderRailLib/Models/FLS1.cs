using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("FLS1")]
    [Serializable]
    public sealed class FLS1 : Job
    {
        private FLS1(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
