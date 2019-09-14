using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("FFEDE")]
    [Serializable]
    public sealed class FFEDE : LNCDLE
    {
        private FFEDE(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
