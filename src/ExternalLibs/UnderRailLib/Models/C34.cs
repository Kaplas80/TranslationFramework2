using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("C34")]
    [Serializable]
    public sealed class C34 : BaseAction
    {
        private C34(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _qn = info.GetString("C34:QN");
            _pe = info.GetBoolean("C34:PE");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("C34:QN", _qn);
            info.AddValue("C34:PE", _pe);
        }

        private bool _pe = true;

        private string _qn;
    }
}
