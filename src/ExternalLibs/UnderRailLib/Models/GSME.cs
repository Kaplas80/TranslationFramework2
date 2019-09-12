using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("GSME")]
    [Serializable]
    public sealed class GSME : Job
    {
        private GSME(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
