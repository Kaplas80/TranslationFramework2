using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("FAPA")]
    [Serializable]
    public sealed class FAPA : BaseAction, ISerializable
    {
        private FAPA(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
