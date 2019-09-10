using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Enums;

namespace UnderRailLib.Models
{
    [EncodedTypeName("U01")]
    [Serializable]
    public sealed class U01 : Job
    {
        private U01(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _s = info.GetString("U01:S");
            _t = info.GetString("U01:T");
            _a = (eATTI) info.GetValue("U01:A", typeof(eATTI));
            _u = (info.GetValue("U01:U", typeof(eATTI?)) as eATTI?);
            _v = info.GetBoolean("U01:V");
            _d = info.GetBoolean("U01:D");
            _c = info.GetBoolean("U01:C");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("U01:S", _s);
            info.AddValue("U01:T", _t);
            info.AddValue("U01:A", _a);
            info.AddValue("U01:U", _u);
            info.AddValue("U01:V", _v);
            info.AddValue("U01:D", _d);
            info.AddValue("U01:C", _c);
        }

        private string _s;

        private string _t;

        private eATTI _a;

        private eATTI? _u;

        private bool _v;

        private bool _d;

        private bool _c;
    }
}
