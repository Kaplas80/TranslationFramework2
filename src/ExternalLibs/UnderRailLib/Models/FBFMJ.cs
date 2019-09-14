using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("FBFMJ")]
    [Serializable]
    public sealed class FBFMJ : Job
    {
        private FBFMJ(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
