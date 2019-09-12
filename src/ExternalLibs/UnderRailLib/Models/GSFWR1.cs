using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("GSFWR1")]
    [Serializable]
    public sealed class GSFWR1 : Job
    {
        private GSFWR1(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
