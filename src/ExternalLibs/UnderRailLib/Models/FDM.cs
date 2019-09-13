using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("FDM")]
    [Serializable]
    public sealed class FDM : Job
    {
        private FDM(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
