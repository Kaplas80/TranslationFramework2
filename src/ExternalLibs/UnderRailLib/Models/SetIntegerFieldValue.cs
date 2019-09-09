using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("SIFV")]
    [Serializable]
    public sealed class SetIntegerFieldValue : SetFieldValueAction
    {
        private SetIntegerFieldValue(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MajorVersion != 0)
            {
                if (DataModelVersion.MajorVersion >= 34)
                {
                    _value = (info.GetValue("SIFV:V", typeof(int?)) as int?);
                    return;
                }
                _value = info.GetInt32("SIFV:V");
            }
            else
            {
                if (GetType() == typeof(SetIntegerFieldValue))
                {
                    _value = info.GetInt32("_Value");
                    return;
                }
                _value = info.GetInt32("SetIntegerFieldValue+_Value");
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("SIFV:V", _value);
        }

        private int? _value;
    }

}
