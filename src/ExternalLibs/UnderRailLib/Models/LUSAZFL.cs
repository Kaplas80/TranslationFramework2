using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("LUSAZFL")]
    [Serializable]
    public sealed class LUSAZFL : Job
    {
        private LUSAZFL(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
