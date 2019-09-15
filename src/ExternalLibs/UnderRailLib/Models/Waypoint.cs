using System;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Interfaces;
using UnderRailLib.TimelapseVertigo.Playfield.Space;

namespace UnderRailLib.Models
{
    [EncodedTypeName("W")]
    [Serializable]
    public class Waypoint : iIC, iNM, iLY, ISerializable
    {
        protected Waypoint(SerializationInfo info, StreamingContext ctx)
        {
            if (DataModelVersion.MajorVersion != 0)
            {
                _name = info.GetString("W:N");
                _areaName = info.GetString("W:AN");
                _location = (Vector3)info.GetValue("W:L", typeof(Vector3));
                _direction = (HorizontalDirection)info.GetValue("W:D", typeof(HorizontalDirection));
                _zone = (Zone)info.GetValue("W:Z", typeof(Zone));
                _layer = info.GetInt32("W:L1");
                return;
            }
            if (GetType() == typeof(Waypoint))
            {
                _name = info.GetString("_Name");
                _areaName = info.GetString("_AreaName");
                _location = (Vector3)info.GetValue("_Location", typeof(Vector3));
                _direction = (HorizontalDirection)info.GetValue("_Direction", typeof(HorizontalDirection));
                _zone = (Zone)info.GetValue("_Zone", typeof(Zone));
                _layer = info.GetInt32("_Layer");
                return;
            }
            _name = info.GetString("Waypoint+_Name");
            _areaName = info.GetString("Waypoint+_AreaName");
            _location = (Vector3)info.GetValue("Waypoint+_Location", typeof(Vector3));
            _direction = (HorizontalDirection)info.GetValue("Waypoint+_Direction", typeof(HorizontalDirection));
            _zone = (Zone)info.GetValue("Waypoint+_Zone", typeof(Zone));
            _layer = info.GetInt32("Waypoint+_Layer");
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            info.AddValue("W:N", _name);
            info.AddValue("W:AN", _areaName);
            info.AddValue("W:L", _location);
            info.AddValue("W:D", _direction);
            info.AddValue("W:Z", _zone);
            info.AddValue("W:L1", _layer);
        }

        private string _name;

        private string _areaName;

        private Vector3 _location;

        private HorizontalDirection _direction;

        private Zone _zone;

        private int _layer;
    }
}
