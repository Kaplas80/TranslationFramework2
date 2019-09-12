using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("XPBLAMC")]
    [Serializable]
    public sealed class XPBLAMC : Condition
    {
        private XPBLAMC(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _mbv = info.GetInt32("XPBLAMC:MBV");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("XPBLAMC:MBV", _mbv);
        }

        private int _mbv;
    }
}