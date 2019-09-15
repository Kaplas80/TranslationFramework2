using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("C00")]
    [Serializable]
    public sealed class C00 : NonEquippableItem
    {
        private C00(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
