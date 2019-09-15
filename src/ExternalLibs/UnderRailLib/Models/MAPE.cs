using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("MAPE")]
    [Serializable]
    public sealed class MAPE : SE2
    {
        private MAPE(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _amount = info.GetInt32("MAPE:A");
            if (DataModelVersion.MinorVersion >= 144)
            {
                _applyPenalties = info.GetBoolean("MAPE:P");
            }
            if (DataModelVersion.MinorVersion >= 525)
            {
                _disableInStealth = info.GetBoolean("MAPE:DIS");
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("MAPE:A", _amount);
            if (DataModelVersion.MinorVersion >= 144)
            {
                info.AddValue("MAPE:P", _applyPenalties);
            }

            if (DataModelVersion.MinorVersion >= 525)
            {
                info.AddValue("MAPE:DIS", _disableInStealth);
            }
        }

        public int _amount;

        private bool _applyPenalties = true;

        public bool _disableInStealth;
    }
}
