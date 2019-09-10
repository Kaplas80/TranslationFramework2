using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("CISMLC")]
    [Serializable]
    public sealed class CISMLC : Condition, ISerializable
    {
        private CISMLC(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
