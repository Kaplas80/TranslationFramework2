using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("MCST")]
    [Serializable]
    public sealed class MCST : Condition
    {
        private MCST(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _m = info.GetBoolean("MCST:M");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("MCST:M", _m);
        }

        private bool _m;
    }
}
