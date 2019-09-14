using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("XPBLMPC")]
    [Serializable]
    public sealed class XPBLMPC : Job
    {
        private XPBLMPC(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
