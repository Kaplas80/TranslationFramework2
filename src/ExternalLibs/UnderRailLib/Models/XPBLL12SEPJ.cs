using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("XPBLL12SEPJ")]
    [Serializable]
    public sealed class XPBLL12SEPJ : Job
    {
        private XPBLL12SEPJ(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
