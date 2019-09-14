using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("CCRBID")]
    [Serializable]
    public sealed class CCRBID : Job
    {
        private CCRBID(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
