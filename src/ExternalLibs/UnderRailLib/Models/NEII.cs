using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.TimelapseVertigo.Rules.Items;

namespace UnderRailLib.Models
{
    [EncodedTypeName("NEII")]
    [Serializable]
    public class NEII : II, IItemInstance<NonEquippableItem>
    {
        protected NEII(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
