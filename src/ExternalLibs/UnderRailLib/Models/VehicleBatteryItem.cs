using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("VBI")]
    [Serializable]
    public sealed class VehicleBatteryItem : VehiclePartItem
    {
        private VehicleBatteryItem(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
