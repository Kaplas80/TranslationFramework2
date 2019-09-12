using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("CHSEC")]
    [Serializable]
    public sealed class CHSEC : Condition
    {
        private CHSEC(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _cip = info.GetBoolean("CHSEC:CIP");
            _i = (Guid?) info.GetValue("CHSEC:I", typeof(Guid?));
            _a = info.GetString("CHSEC:A");
            _n = info.GetBoolean("CHSEC:N");
            _s = info.GetString("CHSEC:S");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("CHSEC:CIP", _cip);
            info.AddValue("CHSEC:I", _i);
            info.AddValue("CHSEC:A", _a);
            info.AddValue("CHSEC:N", _n);
            info.AddValue("CHSEC:S", _s);
        }

        private bool _cip;

        private Guid? _i;

        private string _a;

        private bool _n;

        private string _s;
    }
}