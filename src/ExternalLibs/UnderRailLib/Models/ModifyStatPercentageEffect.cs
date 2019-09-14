using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("MSPE")]
    [Serializable]
    public abstract class ModifyStatPercentageEffect : StatusEffect
    {
        protected ModifyStatPercentageEffect(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                _value = info.GetInt32("MSPE:V");
                return;
            }
            if (GetType() == typeof(ModifyStatPercentageEffect))
            {
                _value = info.GetInt32("_Value");
                return;
            }
            _value = info.GetInt32("ModifyStatPercentageEffect+_Value");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("MSPE:V", _value);
        }

        private int _value;
    }
}
