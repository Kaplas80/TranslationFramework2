using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Enums;

namespace UnderRailLib.Models
{
    [EncodedTypeName("ICC")]
    [Serializable]
    public class IntegerComparisonCondition : ContextInquiryCondition
    {
        protected IntegerComparisonCondition(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MajorVersion != 0)
            {
                if (DataModelVersion.MajorVersion >= 34)
                {
                    _value = (info.GetValue("ICC:V", typeof(int?)) as int?);
                }
                else
                {
                    _value = info.GetInt32("ICC:V");
                }

                _comparationType = (ComparationType2) info.GetValue("ICC:CT", typeof(ComparationType2));
                return;
            }

            if (GetType() == typeof(IntegerComparisonCondition))
            {
                _value = info.GetInt32("_Value");
                _comparationType = (ComparationType2) info.GetValue("_ComparationType", typeof(ComparationType2));
                return;
            }

            _value = info.GetInt32("IntegerComaprisonCondition+_Value");
            _comparationType = (ComparationType2) info.GetValue("IntegerComaprisonCondition+_ComparationType", typeof(ComparationType2));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("ICC:V", _value);
            info.AddValue("ICC:CT", _comparationType);
        }

        private int? _value = 0;
        private ComparationType2 _comparationType;
    }
}
