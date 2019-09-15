using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("FEA")]
    [Serializable]
    public sealed class FEA : SEA, ISerializable
    {
        private FEA(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
