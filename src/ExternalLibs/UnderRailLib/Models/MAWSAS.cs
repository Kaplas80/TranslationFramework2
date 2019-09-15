using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.TimelapseVertigo.Rules.Combat;

namespace UnderRailLib.Models
{
    [EncodedTypeName("MAWSAS")]
    [Serializable]
    public sealed class MAWSAS : SE2
    {
        private MAWSAS(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _a = info.GetSingle("MAWSAS:A");
            _subtype = (WeaponAggregateSubtype)info.GetValue("MAWSAS:S", typeof(WeaponAggregateSubtype));
            _c = info.GetInt32("MAWSAS:C");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("MAWSAS:A", _a);
            info.AddValue("MAWSAS:S", _subtype);
            info.AddValue("MAWSAS:C", _c);
        }

        private float _a;

        private WeaponAggregateSubtype _subtype;

        private int _c;
    }
}
