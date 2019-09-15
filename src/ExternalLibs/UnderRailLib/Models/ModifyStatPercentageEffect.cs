using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("MSPE")]
    [Serializable]
    public abstract class ModifyStatPercentageEffect : SE2
    {
        protected ModifyStatPercentageEffect(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                _value = info.GetSingle("MSPE:V");
                return;
            }
            if (GetType() == typeof(ModifyStatPercentageEffect))
            {
                _value = info.GetSingle("_Value");
                return;
            }
            _value = info.GetSingle("ModifyStatPercentageEffect+_Value");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("MSPE:V", _value);
        }

        private float _value;
    }
}
