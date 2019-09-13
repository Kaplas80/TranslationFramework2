using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("CCRCC")]
    [Serializable]
    public sealed class CCRCC : Condition
    {
        private CCRCC(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _i = (Guid?) info.GetValue("CCRCC:I", typeof(Guid?));
            _a = info.GetString("CCRCC:A");
            _n = info.GetBoolean("CCRCC:N");
            _rcc = info.GetString("CCRCC:RCC");
            _m = info.GetBoolean("CCRCC:M");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("CCRCC:I", _i);
            info.AddValue("CCRCC:A", _a);
            info.AddValue("CCRCC:N", _n);
            info.AddValue("CCRCC:RCC", _rcc);
            info.AddValue("CCRCC:M", _m);
        }

        private Guid? _i;

        private string _a;

        private bool _n;

        private string _rcc;

        private bool _m;
    }
}
