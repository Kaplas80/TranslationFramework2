using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("XPBLFGFNJ")]
    [Serializable]
    public sealed class XPBLFGFNJ : Job
    {
        private XPBLFGFNJ(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
