using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.TimelapseVertigo.Playfield.Locale;
using UnderRailLib.TimelapseVertigo.Playfield.Space;

namespace UnderRailLib.Models
{
    [EncodedTypeName("SEJ")]
    [Serializable]
    public class SpawnEntityJob : Job
    {
        protected SpawnEntityJob(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                _location = (SpatialPointer) info.GetValue("SEJ:L", typeof(SpatialPointer));
                _blueprintPath = info.GetString("SEJ:BP");
                _direction = (HorizontalDirection?) info.GetValue("SEJ:D", typeof(HorizontalDirection?));
                _id = (Guid?) info.GetValue("SEJ:I", typeof(Guid?));
                _name = info.GetString("SEJ:N");
                if (DataModelVersion.MinorVersion >= 33)
                {
                    _pp = info.GetString("SEJ:PP");
                }

                if (DataModelVersion.MinorVersion >= 348)
                {
                    _dp = info.GetBoolean("SEJ:DP");
                }

                if (DataModelVersion.MinorVersion >= 484)
                {
                    _characterState = (info.GetValue("SEJ:CS", typeof(CharacterState?)) as CharacterState?);
                    _cf = (info.GetValue("SEJ:CF", typeof(int?)) as int?);
                }
            }
            else
            {
                if (GetType() == typeof(SpawnEntityJob))
                {
                    _location = (SpatialPointer) info.GetValue("_Location", typeof(SpatialPointer));
                    _blueprintPath = info.GetString("_BlueprintPath");
                    _direction = (HorizontalDirection?) info.GetValue("_Direction", typeof(HorizontalDirection?));
                    _id = (Guid?) info.GetValue("_Id", typeof(Guid?));
                    _name = info.GetString("_Name");
                    return;
                }

                _location = (SpatialPointer) info.GetValue("SpawnEntityJob+_Location", typeof(SpatialPointer));
                _blueprintPath = info.GetString("SpawnEntityJob+_BlueprintPath");
                _direction = (HorizontalDirection?) info.GetValue("SpawnEntityJob+_Direction", typeof(HorizontalDirection?));
                _id = (Guid?) info.GetValue("SpawnEntityJob+_Id", typeof(Guid?));
                _name = info.GetString("SpawnEntityJob+_Name");
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("SEJ:L", _location);
            info.AddValue("SEJ:BP", _blueprintPath);
            if (DataModelVersion.MinorVersion >= 33)
            {
                info.AddValue("SEJ:PP", _pp);
            }
            info.AddValue("SEJ:D", _direction);
            info.AddValue("SEJ:I", _id);
            info.AddValue("SEJ:N", _name);
            if (DataModelVersion.MinorVersion >= 348)
            {
                info.AddValue("SEJ:DP", _dp);
            }

            if (DataModelVersion.MinorVersion >= 484)
            {
                info.AddValue("SEJ:CS", _characterState);
                info.AddValue("SEJ:CF", _cf);
            }
        }

        private SpatialPointer _location;

        private string _blueprintPath;

        private string _pp;

        private HorizontalDirection? _direction;

        private Guid? _id;

        private string _name;

        private bool _dp;

        private CharacterState? _characterState;

        private int? _cf;
    }
}
