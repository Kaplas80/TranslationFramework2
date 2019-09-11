using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("C55")]
    [Serializable]
    public sealed class C55 : ItemGeneratorBase
    {
        private C55(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }

        private int _a;
    }
}
