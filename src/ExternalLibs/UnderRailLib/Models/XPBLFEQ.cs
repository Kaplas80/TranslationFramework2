using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("XPBLFEQ")]
    [Serializable]
    public sealed class XPBLFEQ : Job
    {
        private XPBLFEQ(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
