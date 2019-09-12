using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("CJJ")]
    [Serializable]
    public sealed class CJJ : Job
    {
        private CJJ(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
