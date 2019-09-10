using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("CI2")]
    [Serializable]
    public class ConsumableItem : NonEquippableItem
    {
        protected ConsumableItem(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
