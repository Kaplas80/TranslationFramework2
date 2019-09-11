using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("CCCAHA")]
    [Serializable]
    public sealed class CCCAHA : Job
    {
        private CCCAHA(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
