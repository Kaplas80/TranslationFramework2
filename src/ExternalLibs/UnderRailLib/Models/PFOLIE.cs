using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("PFOLIE")]
    [Serializable]
    public sealed class PFOLIE : IFX
    {
        private PFOLIE(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
