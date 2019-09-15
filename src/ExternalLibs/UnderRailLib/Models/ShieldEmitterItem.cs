using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("SEI")]
    [Serializable]
    public sealed class ShieldEmitterItem : EquippableItem
    {
        private ShieldEmitterItem(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                _shieldVisualColor = (Color)info.GetValue("SEI:SVC", typeof(Color));
                _shieldVisualHitColor = (Color)info.GetValue("SEI:SVHC", typeof(Color));
                SerializationHelper.ReadList("SEI:S", ref _shieldings, info);
                _shieldVisualModel = info.GetString("SEI:SVM");
                if (DataModelVersion.MinorVersion >= 196)
                {
                    _dr = info.GetDouble("SEI:DR");
                }
                else
                {
                    _dr = 0.1;
                }
                if (DataModelVersion.MinorVersion >= 199)
                {
                    _cr = info.GetDouble("SEI:CR");
                    return;
                }
                _cr = 5.0;
            }
            else
            {
                if (GetType() == typeof(ShieldEmitterItem))
                {
                    _shieldVisualColor = (Color)info.GetValue("_ShieldVisualColor", typeof(Color));
                    _shieldVisualHitColor = (Color)info.GetValue("_ShieldVisualHitColor", typeof(Color));
                    _shieldings = (List<S7>)info.GetValue("_Shieldings", typeof(List<S7>));
                    _shieldVisualModel = info.GetString("_ShieldVisualModel");
                    return;
                }
                _shieldVisualColor = (Color)info.GetValue("ShieldEmitterItem+_ShieldVisualColor", typeof(Color));
                _shieldVisualHitColor = (Color)info.GetValue("ShieldEmitterItem+_ShieldVisualHitColor", typeof(Color));
                _shieldings = (List<S7>)info.GetValue("ShieldEmitterItem+_Shieldings", typeof(List<S7>));
                _shieldVisualModel = info.GetString("ShieldEmitterItem+_ShieldVisualModel");
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("SEI:SVC", _shieldVisualColor);
            info.AddValue("SEI:SVHC", _shieldVisualHitColor);
            SerializationHelper.WriteList("SEI:S", _shieldings, info);
            info.AddValue("SEI:SVM", _shieldVisualModel);
            info.AddValue("SEI:DR", _dr);
            info.AddValue("SEI:CR", _cr);
        }

        private Color _shieldVisualColor;

        private Color _shieldVisualHitColor;

        private List<S7> _shieldings = new List<S7>();

        private string _shieldVisualModel;

        private double _dr;

        private double _cr;
    }
}
