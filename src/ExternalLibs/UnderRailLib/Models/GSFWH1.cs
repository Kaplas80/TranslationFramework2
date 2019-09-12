using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("GSFWH1")]
    [Serializable]
    public sealed class GSFWH1 : Job
    {
        private GSFWH1(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
