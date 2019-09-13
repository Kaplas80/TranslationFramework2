using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("D10")]
    [Serializable]
    public sealed class D10 : Job
    {
        private D10(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
