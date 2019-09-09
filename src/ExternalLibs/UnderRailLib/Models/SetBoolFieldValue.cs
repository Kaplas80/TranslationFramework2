using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("SBFV")]
    [Serializable]
    public class SetBoolFieldValue : SetFieldValueAction
    {
        protected SetBoolFieldValue(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MajorVersion != 0)
            {
                _value = info.GetBoolean("SBFV:V");
                return;
            }
            if (GetType() == typeof(SetBoolFieldValue))
            {
                _value = info.GetBoolean("_Value");
                return;
            }
            _value = info.GetBoolean("SetBoolFieldValue+_Value");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("SBFV:V", _value);
        }

        private bool _value;
    }
}
