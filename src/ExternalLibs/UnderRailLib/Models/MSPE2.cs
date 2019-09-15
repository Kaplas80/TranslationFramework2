using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("MSPE2")]
    [Serializable]
    public abstract class MSPE2 : SE2
    {
        protected MSPE2(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _value = info.GetDouble("MSPE2:V");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("MSPE2:V", _value);
        }

        private double _value;
    }
}
