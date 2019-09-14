using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("PIDLE")]
    [Serializable]
    public sealed class PIDLE : LNCDLE
    {
        private PIDLE(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
