using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("M60")]
    [Serializable]
    public sealed class M60 : SE2
    {
        private M60(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _a = info.GetDouble("M60:A");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("M60:A", _a);
        }

        public double _a;
    }
}
