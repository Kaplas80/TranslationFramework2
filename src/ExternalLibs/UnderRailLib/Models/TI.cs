using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("TI")]
    [Serializable]
    public class TI : NonEquippableItem
    {
        protected TI(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
