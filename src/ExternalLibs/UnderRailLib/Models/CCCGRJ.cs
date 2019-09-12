using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("CCCGRJ")]
    [Serializable]
    public sealed class CCCGRJ : Job
    {
        private CCCGRJ(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
