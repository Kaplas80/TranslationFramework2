using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("CASMC")]
    [Serializable]
    public sealed class CASMC : Condition
    {
        private CASMC(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _l = info.GetInt32("CASMC:L");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("CASMC:L", _l);
        }

        private int _l;
    }
}
