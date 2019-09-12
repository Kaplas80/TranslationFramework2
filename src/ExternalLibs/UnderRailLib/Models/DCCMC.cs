using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("DCCMC")]
    [Serializable]
    public sealed class DCCMC : BaseAction
    {
        private DCCMC(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
