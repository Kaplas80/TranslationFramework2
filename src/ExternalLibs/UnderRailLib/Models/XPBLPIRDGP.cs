using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("XPBLPIRDGP")]
    [Serializable]
    public sealed class XPBLPIRDGP : Job
    {
        private XPBLPIRDGP(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
