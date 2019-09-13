using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("D09")]
    [Serializable]
    public sealed class D09 : Job
    {
        private D09(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
