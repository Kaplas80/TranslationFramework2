using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("XPBLSCSG")]
    [Serializable]
    public sealed class XPBLSCSG : Job
    {
        private XPBLSCSG(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
