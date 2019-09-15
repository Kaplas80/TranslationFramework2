using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("M20")]
    [Serializable]
    public sealed class M20 : DRAIM
    {
        private M20(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _b = info.GetDouble("M20:B");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("M20:B", _b);
        }
        
        private double _b;
    }
}
