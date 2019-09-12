using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("GMSJ2")]
    [Serializable]
    public sealed class GMSJ2 : Job
    {
        private GMSJ2(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
