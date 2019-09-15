using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("K20")]
    [Serializable]
    public sealed class K20 : DRAIM
    {
        private K20(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _b = info.GetDouble("K20:B");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("K20:B", _b);
        }

        private double _b;
    }
}
