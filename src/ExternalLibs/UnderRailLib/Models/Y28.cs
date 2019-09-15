using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("Y28")]
    [Serializable]
    public sealed class Y28 : Y26
    {
        private Y28(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
