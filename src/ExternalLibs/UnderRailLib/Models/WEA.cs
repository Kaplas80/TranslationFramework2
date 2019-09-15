using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Interfaces;

namespace UnderRailLib.Models
{
    [EncodedTypeName("WEA")]
    [Serializable]
    public sealed class WEA : SEA, iAWL, ISerializable
    {
        private WEA(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
