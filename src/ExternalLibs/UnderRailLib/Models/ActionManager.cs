using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Interfaces;

namespace UnderRailLib.Models
{
    [EncodedTypeName("AM")]
    [Serializable]
    public sealed class ActionManager : bzk, iIC, ISerializable
    {
        private ActionManager(SerializationInfo info, StreamingContext ctx)
        {
            if (DataModelVersion.MajorVersion != 0)
            {
                _zone = (Zone)info.GetValue("AM:Z", typeof(Zone));
                SerializationHelper.ReadList("AM:A", ref _actions, info);
                return;
            }
            if (GetType() == typeof(ActionManager))
            {
                _zone = (Zone)info.GetValue("_Zone", typeof(Zone));
                _actions = (List<ActionSetup>)info.GetValue("_Actions", typeof(List<ActionSetup>));
                return;
            }
            _zone = (Zone)info.GetValue("ActionManager+_Zone", typeof(Zone));
            _actions = (List<ActionSetup>)info.GetValue("ActionManager+_Actions", typeof(List<ActionSetup>));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            info.AddValue("AM:Z", _zone);
            SerializationHelper.WriteList("AM:A", _actions, info);
        }

        private Zone _zone;

        private List<ActionSetup> _actions = new List<ActionSetup>();
    }
}
