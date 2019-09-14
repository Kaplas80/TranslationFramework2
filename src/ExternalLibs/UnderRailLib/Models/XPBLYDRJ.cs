using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("XPBLYDRJ")]
    [Serializable]
    public sealed class XPBLYDRJ : Job
    {
        private XPBLYDRJ(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
