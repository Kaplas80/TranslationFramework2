using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Interfaces;
using UnderRailLib.TimelapseVertigo.Rules.Combat;

namespace UnderRailLib.Models
{
    [EncodedTypeName("M22")]
    [Serializable]
    public sealed class M22 : OAIM, iSAIM
    {
        private M22(SerializationInfo A_0, StreamingContext A_1) : base(A_0, A_1)
        {
            _a = (DamageType)A_0.GetValue("M22:A", typeof(DamageType));
            _b = A_0.GetDouble("M22:B");
            _c = A_0.GetDouble("M22:C");
            _d = A_0.GetInt32("M22:D");
            _e = A_0.GetInt32("M22:E");
            _f = (DamageRange?)A_0.GetValue("M22:F", typeof(DamageRange?));
            if (DataModelVersion.MinorVersion >= 114)
            {
                _u = A_0.GetBoolean("M22:U");
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("M22:A", _a);
            info.AddValue("M22:B", _b);
            info.AddValue("M22:C", _c);
            info.AddValue("M22:D", _d);
            info.AddValue("M22:E", _e);
            info.AddValue("M22:F", _f);
            info.AddValue("M22:U", _u);
        }

        private DamageType _a;

        private double _b;

        private double _c;

        private int _d;

        private int _e;

        private DamageRange? _f;

        private bool _u;
    }
}
