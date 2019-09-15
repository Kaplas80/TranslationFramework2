using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("AL")]
    [Serializable]
    public sealed class AreaLayer : ISerializable
    {
        private AreaLayer(SerializationInfo info, StreamingContext ctx)
        {
            if (DataModelVersion.MajorVersion != 0)
            {
                _areaId = (Guid)info.GetValue("AL:AI", typeof(Guid));
                SerializationHelper.ReadList("AL:E", ref _entities, info);
                SerializationHelper.ReadList("AL:SLA", ref _softLightAreas, info);
                SerializationHelper.ReadList("AL:D", ref _divinations, info);
                SerializationHelper.ReadList("AL:T", ref _triggers, info);
                return;
            }
            if (GetType() == typeof(AreaLayer))
            {
                _areaId = (Guid)info.GetValue("_AreaId", typeof(Guid));
                _entities = (List<E4>)info.GetValue("_Entities", typeof(List<E4>));
                _softLightAreas = (List<SoftLightArea>)info.GetValue("_SoftLightAreas", typeof(List<SoftLightArea>));
                _divinations = (List<Divination>)info.GetValue("_Divinations", typeof(List<Divination>));
                _triggers = (List<Trigger>)info.GetValue("_Triggers", typeof(List<Trigger>));
                return;
            }
            _areaId = (Guid)info.GetValue("AreaLayer+_AreaId", typeof(Guid));
            _entities = (List<E4>)info.GetValue("AreaLayer+_Entities", typeof(List<E4>));
            _softLightAreas = (List<SoftLightArea>)info.GetValue("AreaLayer+_SoftLightAreas", typeof(List<SoftLightArea>));
            _divinations = (List<Divination>)info.GetValue("AreaLayer+_Divinations", typeof(List<Divination>));
            _triggers = (List<Trigger>)info.GetValue("AreaLayer+_Triggers", typeof(List<Trigger>));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            info.AddValue("AL:AI", _areaId);
            SerializationHelper.WriteList("AL:E", _entities, info);
            SerializationHelper.WriteList("AL:SLA", _softLightAreas, info);
            SerializationHelper.WriteList("AL:D", _divinations, info);
            SerializationHelper.WriteList("AL:T", _triggers, info);
        }

        private Guid _areaId;

        private List<E4> _entities = new List<E4>();

        private List<SoftLightArea> _softLightAreas = new List<SoftLightArea>();

        private List<Divination> _divinations = new List<Divination>();

        private List<Trigger> _triggers = new List<Trigger>();
    }
}
