using System;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Enums;
using UnderRailLib.Models.EventArgs;
using UnderRailLib.Models.Interfaces;

namespace UnderRailLib.Models
{
    [EncodedTypeName("LE2")]
    [Serializable]
    public class LE2 : E4, ISerializable, iCP
    {
        protected LE2(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                _axonometricVisualDirection = info.GetInt32("LE2:AVD");
                _colorOverride = (Color?)info.GetValue("LE2:CO", typeof(Color?));
                _alphaOverride = (float?)info.GetValue("LE2:AO", typeof(float?));
                _so = (float?)info.GetValue("LE2:SO", typeof(float?));
                if (DataModelVersion.MajorVersion >= 5)
                {
                    SerializationHelper.ReadEvent("LE2:AVDC", ref _eAxonometricVisualDirectionChanged, info);
                }
                else
                {
                    _eAxonometricVisualDirectionChanged = (EventHandler<eaPCEA>)info.GetValue("LE2:AVDC", typeof(EventHandler<eaPCEA>));
                }
                if (DataModelVersion.MinorVersion >= 18)
                {
                    _cf = (eLEF)info.GetValue("LE2:CF", typeof(eLEF));
                }
                if (DataModelVersion.MinorVersion >= 490)
                {
                    _d = (info.GetValue("LE2:D", typeof(Point?)) as Point?);
                }
            }
            else
            {
                _axonometricVisualDirection = info.GetInt32("_AxonometricVisualDirection");
                _colorOverride = (info.GetValue("_ColorOverride", typeof(Color?)) as Color?);
                _alphaOverride = (info.GetValue("_AlphaOverride", typeof(float?)) as float?);
                _eAxonometricVisualDirectionChanged = (info.GetValue("e_AxonometricVisualDirectionChanged", typeof(EventHandler<eaPCEA>)) as EventHandler<eaPCEA>);
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("LE2:AVD", _axonometricVisualDirection);
            info.AddValue("LE2:CO", _colorOverride);
            info.AddValue("LE2:AO", _alphaOverride);
            info.AddValue("LE2:SO", _so);
            SerializationHelper.WriteEvent("LE2:AVDC", _eAxonometricVisualDirectionChanged, info);
            info.AddValue("LE2:CF", _cf);
            info.AddValue("LE2:D", _d);
        }

        public eLEF _cf;

        public int _axonometricVisualDirection;

        public Color? _colorOverride;

        public float? _alphaOverride;

        public float? _so;

        public new Point? _d;

        private EventHandler<eaPCEA> _eAxonometricVisualDirectionChanged;
    }
}
