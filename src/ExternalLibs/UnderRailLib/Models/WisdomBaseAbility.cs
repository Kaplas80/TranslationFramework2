using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("WBA1")]
    [Serializable]
    public sealed class WisdomBaseAbility : BaseAbility
    {
        private WisdomBaseAbility(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
