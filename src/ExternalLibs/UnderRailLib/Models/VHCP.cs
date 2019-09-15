using System;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.TimelapseVertigo.Rules.Vehicles;

namespace UnderRailLib.Models
{
    [EncodedTypeName("VHCP")]
    [Serializable]
    public sealed class VHCP : ISerializable
    {
        private VHCP(SerializationInfo info, StreamingContext ctx)
        {
            if (DataModelVersion.MinorVersion >= 424)
            {
                _code = info.GetString("VHCP:C");
                _name = info.GetString("VHCP:N");
                _description = info.GetString("VHCP:D");
                _partType = (VehiclePartType)info.GetValue("VHCP:T", typeof(VehiclePartType));
                _maxSize = info.GetInt32("VHCP:S");
                _optional = info.GetBoolean("VHCP:O");
                if (DataModelVersion.MinorVersion >= 373)
                {
                    _schematicPosition = (Point)info.GetValue("VHCP:P", typeof(Point));
                }
                if (DataModelVersion.MinorVersion >= 412)
                {
                    _livePlacement = info.GetBoolean("VHCP:L");
                }
            }
            else
            {
                _code = info.GetString("_Code");
                _name = info.GetString("_Name");
                _description = info.GetString("_Description");
                _partType = (VehiclePartType)info.GetValue("_PartType", typeof(VehiclePartType));
                _maxSize = info.GetInt32("_MaxSize");
                _optional = info.GetBoolean("_Optional");
                try
                {
                    _schematicPosition = (Point)info.GetValue("_SchematicPosition", typeof(Point));
                }
                catch
                {
                }
                try
                {
                    _livePlacement = info.GetBoolean("_LivePlacement");
                }
                catch
                {
                }
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            info.AddValue("VHCP:C", _code);
            info.AddValue("VHCP:N", _name);
            info.AddValue("VHCP:D", _description);
            info.AddValue("VHCP:T", _partType);
            info.AddValue("VHCP:S", _maxSize);
            info.AddValue("VHCP:O", _optional);
            info.AddValue("VHCP:P", _schematicPosition);
            info.AddValue("VHCP:L", _livePlacement);
        }

        private string _code;

        private string _name;

        private string _description;

        private VehiclePartType _partType;

        private int _maxSize;

        private bool _optional;

        private Point _schematicPosition;

        private bool _livePlacement;
    }
}
