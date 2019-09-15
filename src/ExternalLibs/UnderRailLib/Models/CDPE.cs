using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("CDPE")]
    [Serializable]
    public sealed class CDPE : SE2
    {
        private CDPE(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
