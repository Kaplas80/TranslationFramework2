using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("DCFAC")]
    [Serializable]
    public sealed class DCFAC : Job
    {
        private DCFAC(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
