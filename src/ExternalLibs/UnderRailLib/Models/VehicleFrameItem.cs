using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Enums;

namespace UnderRailLib.Models
{
    [EncodedTypeName("VFI")]
    [Serializable]
    public sealed class VehicleFrameItem : VehiclePartItem
    {
        private VehicleFrameItem(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _v = (eVT)info.GetValue("VFI:V", typeof(eVT));
            _c = info.GetDouble("VFI:C");
            _d = info.GetInt32("VFI:D");
            _t = info.GetDouble("VFI:T");
            _s = info.GetDouble("VFI:S");
            _a = (eCAT)info.GetValue("VFI:A", typeof(eCAT));
            SerializationHelper.ReadList("VFI:R", ref _r, info);
            if (DataModelVersion.MinorVersion >= 372)
            {
                _m = info.GetString("VFI:M");
            }
            if (DataModelVersion.MinorVersion >= 511)
            {
                _n = info.GetDouble("VFI:N");
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("VFI:V", _v);
            info.AddValue("VFI:C", _c);
            info.AddValue("VFI:D", _d);
            info.AddValue("VFI:T", _t);
            info.AddValue("VFI:S", _s);
            info.AddValue("VFI:A", _a);
            SerializationHelper.WriteList("VFI:R", _r, info);
            info.AddValue("VFI:M", _m);
            info.AddValue("VFI:N", _n);
        }

        private eVT _v;

        private double _c = 0.3;

        private int _d = 300;

        private double _t = 0.65;

        private double _n = 0.7;

        private double _s = 1.0;

        private eCAT _a;

        private List<DR2> _r = new List<DR2>();

        private string _m;
    }
}
