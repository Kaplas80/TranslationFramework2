using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("TI")]
    [Serializable]
    public class ToolItem : NonEquippableItem
    {
        protected ToolItem(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
