using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Interfaces;

namespace UnderRailLib.Models
{
    [EncodedTypeName("MMSE")]
    [Serializable]
    public sealed class ModifyMovementSpeedEffect : StatusEffect, iMS
    {
        private ModifyMovementSpeedEffect(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _a = info.GetSingle("MMSE:A");
            _dis = info.GetBoolean("MMSE:DIS");
            _n = info.GetBoolean("MMSE:N");
            if (DataModelVersion.MinorVersion >= 469)
            {
                _i = info.GetBoolean("MMSE:I");
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("MMSE:A", _a);
            info.AddValue("MMSE:DIS", _dis);
            info.AddValue("MMSE:N", _n);
            if (DataModelVersion.MinorVersion >= 469)
            {
                info.AddValue("MMSE:I", _i);
            }
        }

        private float _a;

        private bool _dis;

        private bool _n;

        private bool _i;
    }
}
