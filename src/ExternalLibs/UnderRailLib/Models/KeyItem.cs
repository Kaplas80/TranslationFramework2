using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("KI2")]
    [Serializable]
    public class KeyItem : NonEquippableItem
    {
        protected KeyItem(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
