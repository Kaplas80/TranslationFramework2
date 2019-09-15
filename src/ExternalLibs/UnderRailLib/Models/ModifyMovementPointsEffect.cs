using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Interfaces;

namespace UnderRailLib.Models
{
    [EncodedTypeName("MMPE")]
    [Serializable]
    public sealed class ModifyMovementPointsEffect : StatusEffect, iMS
    {
        private ModifyMovementPointsEffect(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _amount = info.GetInt32("MMPE:A");
            _disableInStealth = info.GetBoolean("MMPE:DIS");
            if (DataModelVersion.MinorVersion >= 144)
            {
                _applyPenalties = info.GetBoolean("MMPE:P");
            }
            if (DataModelVersion.MinorVersion >= 469)
            {
                _ignoreImmunity = info.GetBoolean("MMPE:I");
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("MMPE:A", _amount);
            info.AddValue("MMPE:DIS", _disableInStealth);
            if (DataModelVersion.MinorVersion >= 144)
            {
                info.AddValue("MMPE:P", _applyPenalties);
            }

            if (DataModelVersion.MinorVersion >= 469)
            {
                info.AddValue("MMPE:I", _ignoreImmunity);
            }
        }

        public int _amount;

        public bool _disableInStealth;

        private bool _applyPenalties = true;

        private bool _ignoreImmunity;
    }
}
