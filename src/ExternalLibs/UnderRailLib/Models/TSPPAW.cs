using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("TSPPAW")]
    [Serializable]
    public sealed class TSPPAW : Job
    {
        private TSPPAW(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
