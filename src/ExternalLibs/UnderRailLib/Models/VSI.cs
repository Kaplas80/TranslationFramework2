using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("VSI")]
    [Serializable]
    public abstract class VSI : VehiclePartItem, ISerializable
    {
        protected VSI(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _s = info.GetDouble("VSI:S");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("VSI:S", _s);
        }

        private double _s;
    }
}
