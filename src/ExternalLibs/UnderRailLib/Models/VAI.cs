using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.TimelapseVertigo.Rules.Vehicles;

namespace UnderRailLib.Models
{
    [EncodedTypeName("VAI")]
    [Serializable]
    public sealed class VAI : VehiclePartItem
    {
        private VAI(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _a = (VehiclePartType)info.GetValue("VAI:A", typeof(VehiclePartType));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("VAI:A", _a);
        }

        private VehiclePartType _a;
    }
}
