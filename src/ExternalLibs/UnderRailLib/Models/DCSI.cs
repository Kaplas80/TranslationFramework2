using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("DCSI")]
    [Serializable]
    public sealed class DCSI : Job
    {
        private DCSI(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
