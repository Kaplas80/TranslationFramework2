using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("JYHCJ")]
    [Serializable]
    public sealed class JYHCJ : Job
    {
        private JYHCJ(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
