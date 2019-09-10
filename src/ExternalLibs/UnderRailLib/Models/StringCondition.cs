using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("SC1")]
    [Serializable]
    public class StringCondition : ContextInquiryCondition
    {
        protected StringCondition(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MajorVersion != 0)
            {
                _value = info.GetString("SC1:V");
                if (DataModelVersion.MajorVersion >= 25)
                {
                    _i = info.GetBoolean("SC1:I");
                }
            }
            else
            {
                if (GetType() == typeof(StringCondition))
                {
                    _value = info.GetString("_Value");
                    return;
                }

                _value = info.GetString("StringCondition+_Value");
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("SC1:V", _value);
            if (DataModelVersion.MajorVersion >= 25)
            {
                info.AddValue("SC1:I", _i);
            }
        }

        private string _value;

        private bool _i;
    }
}
