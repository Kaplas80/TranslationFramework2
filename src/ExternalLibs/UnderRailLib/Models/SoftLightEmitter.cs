using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Interfaces;

namespace UnderRailLib.Models
{
    [EncodedTypeName("SLE")]
    [Serializable]
    public sealed class SoftLightEmitter : iLFR, iID, iNM, iLY, ISerializable
    {
        private SoftLightEmitter(SerializationInfo info, StreamingContext ctx)
        {
            if (DataModelVersion.MajorVersion != 0)
            {
                SerializationHelper.ReadList("SLE:A", ref _areas, info);
                _enabled = info.GetBoolean("SLE:E");
                _layer = info.GetInt32("SLE:L");
                _id = (Guid)info.GetValue("SLE:I", typeof(Guid));
                _name = info.GetString("SLE:N");
                _light = (Light)info.GetValue("SLE:L1", typeof(Light));
                return;
            }
            if (GetType() == typeof(SoftLightEmitter))
            {
                _areas = (List<SoftLightArea>)info.GetValue("_Areas", typeof(List<SoftLightArea>));
                _enabled = info.GetBoolean("_Enabled");
                _layer = info.GetInt32("_Layer");
                _id = (Guid)info.GetValue("<Id>k__BackingField", typeof(Guid));
                _name = info.GetString("<Name>k__BackingField");
                _light = (Light)info.GetValue("<Light>k__BackingField", typeof(Light));
                return;
            }
            _areas = (List<SoftLightArea>)info.GetValue("SoftLightEmitter+_Areas", typeof(List<SoftLightArea>));
            _enabled = info.GetBoolean("SoftLightEmitter+_Enabled");
            _layer = info.GetInt32("SoftLightEmitter+_Layer");
            _id = (Guid)info.GetValue("SoftLightEmitter+<Id>k__BackingField", typeof(Guid));
            _name = info.GetString("SoftLightEmitter+<Name>k__BackingField");
            _light = (Light)info.GetValue("SoftLightEmitter+<Light>k__BackingField", typeof(Light));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            SerializationHelper.WriteList("SLE:A", _areas, info);
            info.AddValue("SLE:E", _enabled);
            info.AddValue("SLE:L", _layer);
            info.AddValue("SLE:I", _id);
            info.AddValue("SLE:N", _name);
            info.AddValue("SLE:L1", _light);
        }

        private List<SoftLightArea> _areas = new List<SoftLightArea>();

        private bool _enabled = true;

        private int _layer;

        private Guid _id;

        private string _name;

        private Light _light;
    }
}
