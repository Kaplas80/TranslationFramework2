using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("SPG")]
    [Serializable]
    public sealed class SPG : WCGB
    {
        private SPG(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
