using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("BC")]
    [Serializable]
    public class BoolCondition : ContextInquiryCondition
    {
        protected BoolCondition(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MajorVersion != 0)
            {
                _value = info.GetBoolean("BC:V");
                return;
            }
            if (GetType() == typeof(BoolCondition))
            {
                _value = info.GetBoolean("_Value");
                return;
            }
            _value = info.GetBoolean("BoolCondition+_Value");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("BC:V", _value);
        }

        private bool _value;
    }
}
