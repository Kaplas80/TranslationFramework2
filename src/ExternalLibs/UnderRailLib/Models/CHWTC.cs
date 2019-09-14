using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Enums;
using UnderRailLib.TimelapseVertigo.Rules.Combat;

namespace UnderRailLib.Models
{
    [EncodedTypeName("CHWTC")]
    [Serializable]
    public sealed class CHWTC : Condition
    {
        private CHWTC(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _i = (Guid?)info.GetValue("CHWTC:I", typeof(Guid?));
            _a = info.GetString("CHWTC:A");
            _n = info.GetBoolean("CHWTC:N");
            _b = info.GetBoolean("CHWTC:B");
            _type = (WeaponType)info.GetValue("CHWTC:T", typeof(WeaponType));
            _subtype = (info.GetValue("CHWTC:S", typeof(WeaponSubtype?)) as WeaponSubtype?);
            if (DataModelVersion.MinorVersion >= 520)
            {
                _St = (info.GetValue("CHWTC:ST", typeof(eSPCJBT?)) as eSPCJBT?);
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("CHWTC:I", _i);
            info.AddValue("CHWTC:A", _a);
            info.AddValue("CHWTC:N", _n);
            info.AddValue("CHWTC:B", _b);
            info.AddValue("CHWTC:T", _type);
            info.AddValue("CHWTC:S", _subtype);
            info.AddValue("CHWTC:ST", _St);
        }

        private Guid? _i;

        private string _a;

        private bool _n;

        private bool _b;

        private WeaponType _type;

        private WeaponSubtype? _subtype;

        private eSPCJBT? _St;
    }
}
