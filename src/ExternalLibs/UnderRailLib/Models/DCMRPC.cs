using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("DCMRPC")]
    [Serializable]
    public sealed class DCMRPC : Condition
    {
        private DCMRPC(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _i = info.GetInt32("DCMRPC:I");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("DCMRPC:I", _i);
        }

        private int _i;
    }
}
