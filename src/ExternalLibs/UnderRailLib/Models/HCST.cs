using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("HCST")]
    [Serializable]
    public sealed class HCST : Condition
    {
        private HCST(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _h = info.GetBoolean("HCST:H");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("HCST:H", _h);
        }

        private bool _h;
    }
}
