using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("SHG")]
    [Serializable]
    public sealed class SHG : WCGB
    {
        private SHG(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
