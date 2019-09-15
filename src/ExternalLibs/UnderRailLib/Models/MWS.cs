using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.TimelapseVertigo.Rules.Combat;

namespace UnderRailLib.Models
{
    [EncodedTypeName("MWS")]
    [Serializable]
    public sealed class MWS : SE2
    {
        private MWS(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _a = info.GetSingle("MWS:A");
            _s = (WeaponSubtype)info.GetValue("MWS:S", typeof(WeaponSubtype));
            _p = info.GetInt32("MWS:P");
            _c = info.GetInt32("MWS:C");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("MWS:A", _a);
            info.AddValue("MWS:S", _s);
            info.AddValue("MWS:P", _p);
            info.AddValue("MWS:C", _c);
        }

        private float _a;

        private WeaponSubtype _s;

        private int _p;

        private int _c;
    }
}
