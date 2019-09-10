using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("MI")]
    [Serializable]
    public class MedicineItem : ConsumableItem
    {
        protected MedicineItem(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
