using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("YE")]
    [Serializable]
    public sealed class YE : Condition
    {
        private YE(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _i = (Guid?) info.GetValue("YE:I", typeof(Guid?));
            _a = info.GetString("YE:A");
            _n = info.GetBoolean("YE:N");
            _w = info.GetString("YE:W");
            _d = info.GetBoolean("YE:D");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("YE:I", _i);
            info.AddValue("YE:A", _a);
            info.AddValue("YE:N", _n);
            info.AddValue("YE:W", _w);
            info.AddValue("YE:D", _d);
        }

        private Guid? _i;

        private string _a;

        private bool _n;

        private string _w;

        private bool _d;
    }
}
