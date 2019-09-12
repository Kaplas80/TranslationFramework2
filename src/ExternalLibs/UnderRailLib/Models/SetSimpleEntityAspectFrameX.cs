using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("SSEAFX")]
    [Serializable]
    public sealed class SetSimpleEntityAspectFrameX : Job
    {
        private SetSimpleEntityAspectFrameX(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                _value = info.GetInt32("SSEAFX:V");
                return;
            }
            if (GetType() == typeof(SetSimpleEntityAspectFrameX))
            {
                _value = info.GetInt32("_Value");
                return;
            }
            _value = info.GetInt32("SetSimpleEntityAspectFrameX+_Value");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("SSEAFX:V", _value);
        }

        private int _value;
    }
}
