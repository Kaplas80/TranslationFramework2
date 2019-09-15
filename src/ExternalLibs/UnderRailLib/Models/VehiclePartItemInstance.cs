using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.TimelapseVertigo.Rules.Items;

namespace UnderRailLib.Models
{
    [EncodedTypeName("VPII")]
    [Serializable]
    public sealed class VehiclePartItemInstance : ItemInstance, IItemInstance<VehiclePartItem>, ISerializable
    {
        private VehiclePartItemInstance(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
