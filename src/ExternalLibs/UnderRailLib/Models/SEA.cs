using System;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Interfaces;
using UnderRailLib.Ouroboros.Common.Aspects;

namespace UnderRailLib.Models
{
    [EncodedTypeName("SEA")]
    [Serializable]
    public class SEA : Aspect<LE2>, iLSTA, iOFBEA, ISerializable
    {
        protected SEA(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                a = info.GetString("SEA:M");
                _frame = (Point)info.GetValue("SEA:F", typeof(Point));
                _colorOverride = (Color?)info.GetValue("SEA:CO", typeof(Color?));
                _alphaOverride = (float?)info.GetValue("SEA:AO", typeof(float?));
                if (DataModelVersion.MinorVersion >= 64)
                {
                    _t = info.GetString("SEA:T");
                }
                if (DataModelVersion.MinorVersion >= 68)
                {
                    _b = info.GetSingle("SEA:B");
                }
                else
                {
                    _b = 1f;
                }
                if (DataModelVersion.MinorVersion >= 481)
                {
                    _d = info.GetBoolean("SEA:D");
                }
                if (DataModelVersion.MinorVersion >= 543)
                {
                    _l = (info.GetValue("SEA:L", typeof(float?)) as float?);
                }
            }
            else
            {
                a = info.GetString("_Model");
                _frame = (Point)info.GetValue("_Frame", typeof(Point));
                _colorOverride = (info.GetValue("_ColorOverride", typeof(Color?)) as Color?);
                try
                {
                    _alphaOverride = (info.GetValue("_AlphaOverride", typeof(float?)) as float?);
                }
                catch
                {
                    byte? b = info.GetValue("_AlphaOverride", typeof(byte?)) as byte?;
                    if (b != null)
                    {
                        _alphaOverride = b.Value / 255f;
                    }
                }
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("SEA:M", a);
            info.AddValue("SEA:F", _frame);
            info.AddValue("SEA:CO", _colorOverride);
            info.AddValue("SEA:AO", _alphaOverride);
            info.AddValue("SEA:T", _t);
            info.AddValue("SEA:B", _b);
            info.AddValue("SEA:D", _d);
            info.AddValue("SEA:L", _l);
        }

        public string a;

        public Point _frame;

        public Color? _colorOverride;

        public float? _alphaOverride;

        public string _t;

        public float _b = 1f;

        public float? _l;

        public bool _d;
    }
}
