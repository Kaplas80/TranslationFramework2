using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("VEI")]
    [Serializable]
    public sealed class VehicleEngineItem : VehiclePartItem
    {
        private VehicleEngineItem(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _p = info.GetInt32("VEI:P");
            _c = info.GetDouble("VEI:C");
            _s = info.GetString("VEI:S");
            _v = info.GetSingle("VEI:V");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("VEI:P", _p);
            info.AddValue("VEI:C", _c);
            info.AddValue("VEI:S", _s);
            info.AddValue("VEI:V", _v);
        }

        private int _p = 100;

        private double _c = 1.0;

        private string _s;

        private float _v;
    }
}
