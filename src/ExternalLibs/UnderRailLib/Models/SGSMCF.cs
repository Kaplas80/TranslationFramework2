using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("SGSMCF")]
    [Serializable]
    public sealed class SGSMCF : Job
    {
        private SGSMCF(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
