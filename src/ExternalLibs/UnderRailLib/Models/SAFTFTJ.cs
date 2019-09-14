using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("SAFTFTJ")]
    [Serializable]
    public sealed class SAFTFTJ : Job
    {
        private SAFTFTJ(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
