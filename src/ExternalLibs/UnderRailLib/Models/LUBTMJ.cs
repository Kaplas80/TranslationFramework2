using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("LUBTMJ")]
    [Serializable]
    public sealed class LUBTMJ : Job
    {
        private LUBTMJ(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
