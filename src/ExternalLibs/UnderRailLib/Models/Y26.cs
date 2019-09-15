using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("Y26")]
    [Serializable]
    public class Y26 : AIM
    {
        protected Y26(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _reduction = info.GetDouble("Y26:M");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("Y26:M", _reduction);
        }

        private double _reduction;
    }
}
