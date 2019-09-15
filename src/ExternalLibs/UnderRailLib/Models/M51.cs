using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("M51")]
    [Serializable]
    public sealed class M51 : SE2
    {
        private M51(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _a = info.GetSingle("M51:A");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("M51:A", _a);
        }

        public float _a;
    }
}
