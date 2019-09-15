using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("M59")]
    [Serializable]
    public sealed class M59 : StatusEffect
    {
        private M59(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _a = info.GetDouble("M59:A");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("M59:A", _a);
        }

        public double _a;
    }
}
