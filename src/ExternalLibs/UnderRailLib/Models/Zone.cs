using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Interfaces;
using UnderRailLib.Ouroboros.Common.Data;

namespace UnderRailLib.Models
{
    [EncodedTypeName("Z")]
    [Serializable]
    public class Zone : E2, iIC
    {
        protected Zone(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MajorVersion != 0)
            {
                _customProperties = (PropertyCollection) info.GetValue("Z:CP", typeof(PropertyCollection));
                SerializationHelper.ReadList("Z:A", ref _areas, info);
                SerializationHelper.ReadList("Z:SLE", ref _softLightEmitters, info);
                SerializationHelper.ReadList("Z:LD", ref _lanternDefinitions, info);
                SerializationHelper.ReadList("Z:ED", ref _eyeDefinitions, info);
                _elapsedZoneTime = (TimeSpan) info.GetValue("Z:EZT", typeof(TimeSpan));
                _actionManager = (ActionManager) info.GetValue("Z:AM", typeof(ActionManager));
                SerializationHelper.ReadList("Z:W", ref _waypoints, info);
                return;
            }

            if (GetType() == typeof(Zone))
            {
                _customProperties = (PropertyCollection) info.GetValue("_CustomProperties", typeof(PropertyCollection));
                _areas = (List<Area>) info.GetValue("_Areas", typeof(List<Area>));
                _softLightEmitters = (List<SoftLightEmitter>) info.GetValue("_SoftLightEmitters", typeof(List<SoftLightEmitter>));
                _lanternDefinitions = (List<LanternDefinition>) info.GetValue("_LanternDefinitions", typeof(List<LanternDefinition>));
                _eyeDefinitions = (List<EyeDefinition>) info.GetValue("_EyeDefinitions", typeof(List<EyeDefinition>));
                _elapsedZoneTime = (TimeSpan) info.GetValue("_ElapsedZoneTime", typeof(TimeSpan));
                _actionManager = (ActionManager) info.GetValue("_ActionManager", typeof(ActionManager));
                _waypoints = (List<Waypoint>) info.GetValue("_Waypoints", typeof(List<Waypoint>));
                return;
            }

            _customProperties = (PropertyCollection) info.GetValue("Zone+_CustomProperties", typeof(PropertyCollection));
            _areas = (List<Area>) info.GetValue("Zone+_Areas", typeof(List<Area>));
            _softLightEmitters = (List<SoftLightEmitter>) info.GetValue("Zone+_SoftLightEmitters", typeof(List<SoftLightEmitter>));
            _lanternDefinitions = (List<LanternDefinition>) info.GetValue("Zone+_LanternDefinitions", typeof(List<LanternDefinition>));
            _eyeDefinitions = (List<EyeDefinition>) info.GetValue("Zone+_EyeDefinitions", typeof(List<EyeDefinition>));
            _elapsedZoneTime = (TimeSpan) info.GetValue("Zone+_ElapsedZoneTime", typeof(TimeSpan));
            _actionManager = (ActionManager) info.GetValue("Zone+_ActionManager", typeof(ActionManager));
            _waypoints = (List<Waypoint>) info.GetValue("Zone+_Waypoints", typeof(List<Waypoint>));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("Z:CP", _customProperties);
            SerializationHelper.WriteList("Z:A", _areas, info);
            SerializationHelper.WriteList("Z:SLE", _softLightEmitters, info);
            SerializationHelper.WriteList("Z:LD", _lanternDefinitions, info);
            SerializationHelper.WriteList("Z:ED", _eyeDefinitions, info);
            info.AddValue("Z:EZT", _elapsedZoneTime);
            info.AddValue("Z:AM", _actionManager);
            SerializationHelper.WriteList("Z:W", _waypoints, info);
        }

        private PropertyCollection _customProperties;

        private List<Area> _areas = new List<Area>();

        private List<SoftLightEmitter> _softLightEmitters = new List<SoftLightEmitter>();

        private List<LanternDefinition> _lanternDefinitions = new List<LanternDefinition>();

        private List<EyeDefinition> _eyeDefinitions = new List<EyeDefinition>();

        private TimeSpan _elapsedZoneTime;

        private ActionManager _actionManager;

        private List<Waypoint> _waypoints = new List<Waypoint>();
    }
}
