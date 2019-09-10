using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("SBA")]
    [Serializable]
    public sealed class SBA : BaseAction
    {
        private SBA(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
