using System;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Interfaces;

namespace UnderRailLib.Models
{
    [EncodedTypeName("T")]
    [Serializable]
    public abstract class Trigger : iID, iNM, iIC, iLY, ISerializable
    {
        protected Trigger(SerializationInfo info, StreamingContext ctx)
        {
            if (DataModelVersion.MajorVersion != 0)
            {
                _id = (Guid) info.GetValue("T:I", typeof(Guid));
                _name = info.GetString("T:N");
                _area = (Area) info.GetValue("T:A", typeof(Area));
                _location = (Point) info.GetValue("T:L", typeof(Point));
                _size = (Size) info.GetValue("T:S", typeof(Size));
                _layer = info.GetInt32("T:L1");
                return;
            }

            if (GetType() == typeof(Trigger))
            {
                _id = (Guid) info.GetValue("_Id", typeof(Guid));
                _name = info.GetString("_Name");
                _area = (Area) info.GetValue("_Area", typeof(Area));
                _location = (Point) info.GetValue("_Location", typeof(Point));
                _size = (Size) info.GetValue("_Size", typeof(Size));
                _layer = info.GetInt32("_Layer");
                return;
            }

            _id = (Guid) info.GetValue("Trigger+_Id", typeof(Guid));
            _name = info.GetString("Trigger+_Name");
            _area = (Area) info.GetValue("Trigger+_Area", typeof(Area));
            _location = (Point) info.GetValue("Trigger+_Location", typeof(Point));
            _size = (Size) info.GetValue("Trigger+_Size", typeof(Size));
            _layer = info.GetInt32("Trigger+_Layer");
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            info.AddValue("T:I", _id);
            info.AddValue("T:N", _name);
            info.AddValue("T:A", _area);
            info.AddValue("T:L", _location);
            info.AddValue("T:S", _size);
            info.AddValue("T:L1", _layer);
        }

        private Guid _id;

        private string _name = "UnnamedTrigger";

        private Area _area;

        private Point _location;

        private Size _size;

        private int _layer;
    }
}
