using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("C33")]
    [Serializable]
    public sealed class C33 : BaseAction
    {
        private C33(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _qn = info.GetString("C33:QN");
            _pe = info.GetBoolean("C33:PE");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("C33:QN", _qn);
            info.AddValue("C33:PE", _pe);
        }

        private bool _pe = true;

        private string _qn;
    }
}
