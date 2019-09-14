using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("M42")]
    [Serializable]
    public sealed class M42 : StatusEffect
    {
        private M42(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _a = info.GetDouble("M42:A");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("M42:A", _a);
        }

        public double _a;
    }
}
