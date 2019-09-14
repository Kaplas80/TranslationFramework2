using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("VPI")]
    [Serializable]
    public abstract class VehiclePartItem : NonEquippableItem
    {
        protected VehiclePartItem(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _s = info.GetInt32("VPI:S");
            if (DataModelVersion.MinorVersion >= 381)
            {
                _b = info.GetBoolean("VPI:B");
            }
            if (DataModelVersion.MinorVersion >= 386)
            {
                SerializationHelper.ReadList("VPI:C", ref _c, info);
                return;
            }
            _c = new List<CapabilityReference>();
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("VPI:S", _s);
            info.AddValue("VPI:B", _b);
            SerializationHelper.WriteList("VPI:C", _c, info);
        }

        private int _s;

        private bool _b;

        private List<CapabilityReference> _c = new List<CapabilityReference>();
    }
}
