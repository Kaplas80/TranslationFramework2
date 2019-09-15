using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Interfaces;

namespace UnderRailLib.Models
{
    [EncodedTypeName("M23")]
    [Serializable]
    public sealed class M23 : OAIM, iSAIM
    {
        private M23(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _b = info.GetDouble("M23:B");
            _c = info.GetDouble("M23:C");
            _d = info.GetInt32("M23:D");
            _e = info.GetInt32("M23:E");
            _f = (DamageRange?)info.GetValue("M23:F", typeof(DamageRange?));
            _g = info.GetDouble("M23:G");
            _h = info.GetInt32("M23:H");
            if (DataModelVersion.MinorVersion >= 114)
            {
                _u = info.GetBoolean("M23:U");
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("M23:B", _b);
            info.AddValue("M23:C", _c);
            info.AddValue("M23:D", _d);
            info.AddValue("M23:E", _e);
            info.AddValue("M23:F", _f);
            info.AddValue("M23:G", _g);
            info.AddValue("M23:H", _h);
            info.AddValue("M23:U", _u);
        }

        private double _b;

        private double _c;

        private int _d;

        private int _e;

        private DamageRange? _f;

        private double _g;

        private int _h;

        private bool _u;
    }
}
