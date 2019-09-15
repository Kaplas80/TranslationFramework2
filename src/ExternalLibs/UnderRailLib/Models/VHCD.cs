using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("VHCD")]
    [Serializable]
    public sealed class VHCD : ISerializable
    {
        private VHCD(SerializationInfo info, StreamingContext ctx)
        {
            if (DataModelVersion.MinorVersion >= 423)
            {
                _frame = (info.GetValue("VHCD:F", typeof(VehiclePartItemInstance)) as VehiclePartItemInstance);
                SerializationHelper.ReadList("VHCD:P", ref _parts, info);
                if (DataModelVersion.MinorVersion >= 370)
                {
                    _name = info.GetString("VHCD:N");
                }
                if (DataModelVersion.MinorVersion >= 404)
                {
                    _emitsLight = info.GetBoolean("VHCD:E");
                    _lightColor = (Color)info.GetValue("VHCD:C", typeof(Color));
                }
                if (DataModelVersion.MinorVersion >= 419)
                {
                    _visualSideExtensionBonus = info.GetSingle("VHCD:S");
                    _visualDiagonalExtensionBonus = info.GetSingle("VHCD:D");
                }
                if (DataModelVersion.MinorVersion >= 420)
                {
                    _trailExtensionBonus = info.GetSingle("VHCD:T");
                }
            }
            else
            {
                _frame = (info.GetValue("_Frame", typeof(VehiclePartItemInstance)) as VehiclePartItemInstance);
                _parts = (info.GetValue("_Parts", typeof(List<VHCP>)) as List<VHCP>);
                _name = info.GetString("_Name");
                try
                {
                    _emitsLight = info.GetBoolean("_EmitsLight");
                    _lightColor = (Color)info.GetValue("_LightColor", typeof(Color));
                }
                catch
                {
                }
                try
                {
                    _visualSideExtensionBonus = info.GetSingle("_VisualSideExtensionBonus");
                    _visualDiagonalExtensionBonus = info.GetSingle("_VisualDiagonalExtensionBonus");
                }
                catch
                {
                }
                try
                {
                    _trailExtensionBonus = info.GetSingle("_TrailExtensionBonus");
                }
                catch
                {
                }
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            info.AddValue("VHCD:F", _frame);
            SerializationHelper.WriteList("VHCD:P", _parts, info);
            info.AddValue("VHCD:N", _name);
            info.AddValue("VHCD:E", _emitsLight);
            info.AddValue("VHCD:C", _lightColor);
            info.AddValue("VHCD:S", _visualSideExtensionBonus);
            info.AddValue("VHCD:D", _visualDiagonalExtensionBonus);
            info.AddValue("VHCD:T", _trailExtensionBonus);
        }

        private VehiclePartItemInstance _frame;

        private List<VHCP> _parts = new List<VHCP>();

        private string _name;

        private bool _emitsLight;

        private Color _lightColor;

        private float _visualSideExtensionBonus;

        private float _visualDiagonalExtensionBonus;

        private float _trailExtensionBonus;
    }
}
