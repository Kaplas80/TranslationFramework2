using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("SDSJ1")]
    [Serializable]
    public sealed class SDSJ1 : Job
    {
        private SDSJ1(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _i = (Guid?) info.GetValue("SDSJ1:I", typeof(Guid?));
            _a = info.GetString("SDSJ1:A");
            _l = info.GetBoolean("SDSJ1:L");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("SDSJ1:I", _i);
            info.AddValue("SDSJ1:A", _a);
            info.AddValue("SDSJ1:L", _l);
        }

        private bool _l;

        private Guid? _i;

        private string _a;
    }
}
