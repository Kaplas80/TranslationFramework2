using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("PGFCA")]
    [Serializable]
    public sealed class PGFCA : BaseAction, ISerializable
    {
        private PGFCA(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
