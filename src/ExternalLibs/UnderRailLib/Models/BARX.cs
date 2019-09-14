using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.TimelapseVertigo.Rules.Characters;

namespace UnderRailLib.Models
{
    [EncodedTypeName("BARX")]
    [Serializable]
    public sealed class BARX : RQX
    {
        private BARX(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _b = (BaseAbilityEnum)info.GetValue("BARX:B", typeof(BaseAbilityEnum));
            _m = info.GetInt32("BARX:M");
            _c = info.GetBoolean("BARX:C");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("BARX:B", _b);
            info.AddValue("BARX:M", _m);
            info.AddValue("BARX:C", _c);
        }

        private BaseAbilityEnum _b;

        private bool _c;

        private int _m;
    }
}
