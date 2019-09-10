using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("ISMC")]
    [Serializable]
    public sealed class ISMC : Condition
    {
        private ISMC(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
