using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("ZHH")]
    [Serializable]
    public sealed class ZHH : Condition
    {
        private ZHH(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _i = info.GetBoolean("ZHH:I");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("ZHH:I", _i);
        }

        private bool _i;
    }
}
