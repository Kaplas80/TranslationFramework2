using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("MSFE")]
    [Serializable]
    public abstract class ModifyStatFlatEffect : StatusEffect
    {
        protected ModifyStatFlatEffect(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                _value = info.GetInt32("MSFE:V");
                return;
            }
            if (GetType() == typeof(ModifyStatFlatEffect))
            {
                _value = info.GetInt32("_Value");
                return;
            }
            _value = info.GetInt32("ModifyStatFlatEffect+_Value");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("MSFE:V", _value);
        }

        private int _value;
    }
}
