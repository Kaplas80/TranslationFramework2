using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("FO5VCCC")]
    [Serializable]
    public sealed class FO5VCCC : Condition
    {
        private FO5VCCC(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}