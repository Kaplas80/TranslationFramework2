using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("FI")]
    [Serializable]
    public class FoodItem : ConsumableItem
    {
        protected FoodItem(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
