using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("PGFADIA")]
    [Serializable]
    public sealed class PGFADIA : BaseAction
    {
        private PGFADIA(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}