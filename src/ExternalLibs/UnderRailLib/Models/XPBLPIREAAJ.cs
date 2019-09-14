using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("XPBLPIREAAJ")]
    [Serializable]
    public sealed class XPBLPIREAAJ : Job
    {
        private XPBLPIREAAJ(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
