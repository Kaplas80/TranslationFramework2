using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("FOCSSSJ")]
    [Serializable]
    public sealed class FOCSSSJ : Job
    {
        private FOCSSSJ(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
