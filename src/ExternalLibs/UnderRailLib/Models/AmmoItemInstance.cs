using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.TimelapseVertigo.Rules.Items;

namespace UnderRailLib.Models
{
    [EncodedTypeName("AI3")]
    [Serializable]
    public sealed class AmmoItemInstance : NonEquippableItemInstance, IItemInstance<AmmoItem>
    {
        private AmmoItemInstance(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
