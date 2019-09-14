using System;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Enums;

namespace UnderRailLib.Models
{
    [EncodedTypeName("S15")]
    [Serializable]
    public sealed class S15 : Job
    {
        private S15(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _i = (info.GetValue("S15:I", typeof(int?)) as int?);
            _color = (info.GetValue("S15:C", typeof(Color?)) as Color?);
            _sc = info.GetBoolean("S15:SC");
            _a = (info.GetValue("S15:A", typeof(float?)) as float?);
            _sa = info.GetBoolean("S15:SA");
            if (DataModelVersion.MinorVersion >= 286)
            {
                _fx = info.GetInt32("S15:FX");
                _fy = info.GetInt32("S15:FY");
                _sfx = info.GetBoolean("S15:SFX");
                _sfy = info.GetBoolean("S15:SFY");
            }
            else if (DataModelVersion.MinorVersion >= 233)
            {
                var point = (Point) info.GetValue("S15:F", typeof(Point));
                var boolean = info.GetBoolean("S15:SF");
                _fx = point.X;
                _fy = point.Y;
                _sfx = boolean;
                _sfy = boolean;
            }

            if (DataModelVersion.MinorVersion >= 501)
            {
                _xs = (eSPCJBT) info.GetValue("S15:XS", typeof(eSPCJBT));
                _xa = info.GetString("S15:XA");
                _xi = (info.GetValue("S15:XI", typeof(Guid?)) as Guid?);
            }

            if (DataModelVersion.MinorVersion >= 526)
            {
                _sb = info.GetBoolean("S15:SB");
                _b = info.GetSingle("S15:B");
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("S15:I", _i);
            info.AddValue("S15:C", _color);
            info.AddValue("S15:SC", _sc);
            info.AddValue("S15:A", _a);
            info.AddValue("S15:SA", _sa);
            if (DataModelVersion.MinorVersion >= 286)
            {
                info.AddValue("S15:FX", _fx);
                info.AddValue("S15:FY", _fy);
                info.AddValue("S15:SFX", _sfx);
                info.AddValue("S15:SFY", _sfy);
            }
            else if (DataModelVersion.MinorVersion >= 233)
            {
                var point = new Point
                {
                    X = _fx, Y = _fy
                };
                info.AddValue("S15:F", point);
                info.AddValue("S15:SF", _sfx);
            }
            
            if (DataModelVersion.MinorVersion >= 501)
            {
                info.AddValue("S15:XS", _xs);
                info.AddValue("S15:XA", _xa);
                info.AddValue("S15:XI", _xi);
            }

            if (DataModelVersion.MinorVersion >= 526)
            {
                info.AddValue("S15:SB", _sb);
                info.AddValue("S15:B", _b);
            }
        }

        private int? _i;

        private Color? _color;

        private bool _sc;

        private float? _a;

        private bool _sa;

        private int _fx;

        private int _fy;

        private bool _sfx;

        private bool _sfy;

        private bool _sb;

        private float _b;

        private eSPCJBT _xs = eSPCJBT.c;

        private string _xa;

        private Guid? _xi;
    }
}
