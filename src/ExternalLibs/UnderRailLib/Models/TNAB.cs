using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("TNAB")]
    [Serializable]
    public sealed class TNAB : Job
    {
        private TNAB(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
