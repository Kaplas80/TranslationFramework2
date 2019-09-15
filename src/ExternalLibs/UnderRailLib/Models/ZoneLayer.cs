using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Ouroboros.Common.Data;

namespace UnderRailLib.Models
{
    [EncodedTypeName("ZL")]
    [Serializable]
    public sealed class ZoneLayer : ISerializable
    {
        private ZoneLayer(SerializationInfo info, StreamingContext ctx)
        {
            if (DataModelVersion.MajorVersion != 0)
            {
                _zoneId = (Guid)info.GetValue("ZL:ZI", typeof(Guid));
                SerializationHelper.ReadList("ZL:SLE", ref _softLightEmitters, info);
                SerializationHelper.ReadList("ZL:LD", ref _lanternDefinitions, info);
                SerializationHelper.ReadList("ZL:AL", ref _areaLayers, info);
                SerializationHelper.ReadList("ZL:W", ref _waypoints, info);
                if (DataModelVersion.MajorVersion >= 8)
                {
                    _cp = (info.GetValue("ZL:CP", typeof(PropertyCollection)) as PropertyCollection);
                    return;
                }
                _cp = new PropertyCollection();
            }
            else
            {
                if (GetType() == typeof(ZoneLayer))
                {
                    _zoneId = (Guid)info.GetValue("_ZoneId", typeof(Guid));
                    _softLightEmitters = (List<SoftLightEmitter>)info.GetValue("_SoftLightEmitters", typeof(List<SoftLightEmitter>));
                    _lanternDefinitions = (List<LanternDefinition>)info.GetValue("_LanternDefinitions", typeof(List<LanternDefinition>));
                    _areaLayers = (List<AreaLayer>)info.GetValue("_AreaLayers", typeof(List<AreaLayer>));
                    _waypoints = (List<Waypoint>)info.GetValue("_Waypoints", typeof(List<Waypoint>));
                    return;
                }
                _zoneId = (Guid)info.GetValue("ZoneLayer+_ZoneId", typeof(Guid));
                _softLightEmitters = (List<SoftLightEmitter>)info.GetValue("ZoneLayer+_SoftLightEmitters", typeof(List<SoftLightEmitter>));
                _lanternDefinitions = (List<LanternDefinition>)info.GetValue("ZoneLayer+_LanternDefinitions", typeof(List<LanternDefinition>));
                _areaLayers = (List<AreaLayer>)info.GetValue("ZoneLayer+_AreaLayers", typeof(List<AreaLayer>));
                _waypoints = (List<Waypoint>)info.GetValue("ZoneLayer+_Waypoints", typeof(List<Waypoint>));
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            info.AddValue("ZL:ZI", _zoneId);
            SerializationHelper.WriteList("ZL:SLE", _softLightEmitters, info);
            SerializationHelper.WriteList("ZL:LD", _lanternDefinitions, info);
            SerializationHelper.WriteList("ZL:AL", _areaLayers, info);
            SerializationHelper.WriteList("ZL:W", _waypoints, info);
            info.AddValue("ZL:CP", _cp);
        }

        private Guid _zoneId;

        private List<SoftLightEmitter> _softLightEmitters = new List<SoftLightEmitter>();

        private List<LanternDefinition> _lanternDefinitions = new List<LanternDefinition>();

        private List<AreaLayer> _areaLayers = new List<AreaLayer>();

        private List<Waypoint> _waypoints = new List<Waypoint>();

        private PropertyCollection _cp = new PropertyCollection();
    }
}
