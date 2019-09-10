using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("SSFV")]
    [Serializable]
    public sealed class SetStringFieldValue : SetFieldValueAction
    {
        private SetStringFieldValue(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MajorVersion != 0)
            {
                _value = info.GetString("SSFV:V");
                return;
            }
            if (GetType() == typeof(SetStringFieldValue))
            {
                _value = info.GetString("_Value");
                return;
            }
            _value = info.GetString("SetStringFieldValue+_Value");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("SSFV:V", _value);
        }

        private string _value;
    }

}
