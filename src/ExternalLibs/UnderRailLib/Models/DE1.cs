using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("DE1")]
    [Serializable]
    public sealed class DE1 : SE2
    {
        private DE1(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
