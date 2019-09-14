using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("XPBLNCIC")]
    [Serializable]
    public sealed class XPBLNCIC : Job
    {
        private XPBLNCIC(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
