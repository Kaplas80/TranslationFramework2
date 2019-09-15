using System;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Interfaces;

namespace UnderRailLib.Models
{
    [EncodedTypeName("AE")]
    [Serializable]
    public abstract class AreaEffect : iID, ISerializable
    {
        protected AreaEffect(SerializationInfo info, StreamingContext ctx)
        {
            if (DataModelVersion.MajorVersion != 0)
            {
                _location = (Point) info.GetValue("AE:L", typeof(Point));
                _size = (Size) info.GetValue("AE:S", typeof(Size));
                _lastAppliedLocation = (Point) info.GetValue("AE:LAL", typeof(Point));
                _lastAppliedSize = (Size) info.GetValue("AE:LAS", typeof(Size));
                _lastAppliedEnabled = info.GetBoolean("AE:LAE");
                _id = (Guid) info.GetValue("AE:I", typeof(Guid));
                _name = info.GetString("AE:N");
                return;
            }

            if (GetType() == typeof(AreaEffect))
            {
                _location = (Point) info.GetValue("_Location", typeof(Point));
                _size = (Size) info.GetValue("_Size", typeof(Size));
                _lastAppliedLocation = (Point) info.GetValue("_LastAppliedLocation", typeof(Point));
                _lastAppliedSize = (Size) info.GetValue("_LastAppliedSize", typeof(Size));
                _lastAppliedEnabled = info.GetBoolean("_LastAppliedEnabled");
                _id = (Guid) info.GetValue("<Id>k__BackingField", typeof(Guid));
                _name = info.GetString("<Name>k__BackingField");
                return;
            }

            _location = (Point) info.GetValue("AreaEffect+_Location", typeof(Point));
            _size = (Size) info.GetValue("AreaEffect+_Size", typeof(Size));
            _lastAppliedLocation = (Point) info.GetValue("AreaEffect+_LastAppliedLocation", typeof(Point));
            _lastAppliedSize = (Size) info.GetValue("AreaEffect+_LastAppliedSize", typeof(Size));
            _lastAppliedEnabled = info.GetBoolean("AreaEffect+_LastAppliedEnabled");
            _id = (Guid) info.GetValue("AreaEffect+<Id>k__BackingField", typeof(Guid));
            _name = info.GetString("AreaEffect+<Name>k__BackingField");
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            info.AddValue("AE:L", _location);
            info.AddValue("AE:S", _size);
            info.AddValue("AE:LAL", _lastAppliedLocation);
            info.AddValue("AE:LAS", _lastAppliedSize);
            info.AddValue("AE:LAE", _lastAppliedEnabled);
            info.AddValue("AE:I", _id);
            info.AddValue("AE:N", _name);
        }

        private Point _location;

        private Size _size;

        private Point _lastAppliedLocation;

        private Size _lastAppliedSize;

        private bool _lastAppliedEnabled;

        private Guid _id;

        private string _name;
    }
}
