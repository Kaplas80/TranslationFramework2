using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.TimelapseVertigo.Rules.Items;

namespace UnderRailLib.Models
{
    [EncodedTypeName("SEII")]
    [Serializable]
    public sealed class ShieldEmitterItemInstance : EquippableItemInstance, IItemInstance<ShieldEmitterItem>, ISerializable
    {
        private ShieldEmitterItemInstance(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
