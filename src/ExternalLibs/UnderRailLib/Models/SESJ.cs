using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("SESJ")]
    [Serializable]
    public sealed class SESJ : Job
    {
        private SESJ(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
