using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("ASMA")]
    [Serializable]
    public sealed class ASMA : BaseAction
    {
        private ASMA(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
