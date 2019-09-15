using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Interfaces;
using UnderRailLib.TimelapseVertigo.Rules.Combat;

namespace UnderRailLib.Models
{
    [EncodedTypeName("MDTM")]
    [Serializable]
    public sealed class MDTM : SE2, iMS
    {
        private MDTM(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _value = info.GetDouble("MDTM:V");
            _damageType = (DamageType)info.GetValue("MDTM:DT", typeof(DamageType));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("MDTM:V", _value);
            info.AddValue("MDTM:DT", _damageType);
        }

        private double _value;

        private DamageType _damageType;
    }
}
