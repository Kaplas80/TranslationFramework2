using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("FSMMJ")]
    [Serializable]
    public sealed class FSMMJ : Job
    {
        private FSMMJ(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
