using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("H09")]
    [Serializable]
    public sealed class H09 : Job
    {
        private H09(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
