using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("SGSCIOA")]
    [Serializable]
    public sealed class SGSCIOA : BaseAction
    {
        private SGSCIOA(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
