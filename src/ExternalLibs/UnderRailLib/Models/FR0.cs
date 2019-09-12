using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Enums;

namespace UnderRailLib.Models
{
    [EncodedTypeName("FR0")]
    [Serializable]
    public sealed class FR0 : Condition
    {
        private FR0(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _s = info.GetString("FR0:S");
            _t = info.GetString("FR0:T");
            _a = (eATTI) info.GetValue("FR0:A", typeof(eATTI));
            _c = (ComparationType2) info.GetValue("FR0:C", typeof(ComparationType2));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("FR0:S", _s);
            info.AddValue("FR0:T", _t);
            info.AddValue("FR0:A", _a);
            info.AddValue("FR0:C", _c);
        }

        private string _s;

        private string _t;

        private eATTI _a;

        private ComparationType2 _c;
    }
}
