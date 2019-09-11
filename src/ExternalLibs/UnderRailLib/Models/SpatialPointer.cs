using System;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("SP")]
    [Serializable]
    public sealed class SpatialPointer : ISerializable
    {
        private SpatialPointer(SerializationInfo info, StreamingContext ctx)
        {
            if (DataModelVersion.MajorVersion != 0)
            {
                _location = (Vector3) info.GetValue("SP:L", typeof(Vector3));
                _area = info.GetString("SP:A");
                _waypointName = info.GetString("SP:WN");
                return;
            }

            if (GetType() == typeof(SpatialPointer))
            {
                _location = (Vector3) info.GetValue("_Location", typeof(Vector3));
                _area = info.GetString("_Area");
                _waypointName = info.GetString("_WaypointName");
                return;
            }

            _location = (Vector3) info.GetValue("SpatialPointer+_Location", typeof(Vector3));
            _area = info.GetString("SpatialPointer+_Area");
            _waypointName = info.GetString("SpatialPointer+_WaypointName");
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            info.AddValue("SP:L", _location);
            info.AddValue("SP:A", _area);
            info.AddValue("SP:WN", _waypointName);
        }

        private Vector3 _location;

        private string _area;

        private string _waypointName;
    }
}
