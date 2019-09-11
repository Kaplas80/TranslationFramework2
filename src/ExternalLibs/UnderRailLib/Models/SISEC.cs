using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("SISEC")]
    [Serializable]
    public sealed class SISEC : Condition
    {
        private SISEC(SerializationInfo _info, StreamingContext _ctx) : base(_info, _ctx)
        {
            _i = (Guid?) _info.GetValue("SISEC:I", typeof(Guid?));
            _a = _info.GetString("SISEC:A");
            _n = _info.GetBoolean("SISEC:N");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("SISEC:I", _i);
            info.AddValue("SISEC:A", _a);
            info.AddValue("SISEC:N", _n);
        }

        private Guid? _i;

        private string _a;

        private bool _n;
    }
}
