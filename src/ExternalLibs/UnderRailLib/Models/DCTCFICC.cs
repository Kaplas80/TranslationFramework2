using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("DCTCFICC")]
    [Serializable]
    public sealed class DCTCFICC : Condition
    {
        private DCTCFICC(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}