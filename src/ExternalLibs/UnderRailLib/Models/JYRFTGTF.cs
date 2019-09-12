using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("JYRFTGTF")]
    [Serializable]
    public sealed class JYRFTGTF : Job
    {
        private JYRFTGTF(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
