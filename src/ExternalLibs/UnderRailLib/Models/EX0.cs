using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("EX0")]
    [Serializable]
    public sealed class EX0 : Job
    {
        private EX0(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
