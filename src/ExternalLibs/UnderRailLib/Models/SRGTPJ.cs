using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("SRGTPJ")]
    [Serializable]
    public sealed class SRGTPJ : Job
    {
        private SRGTPJ(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _i = (Guid?)info.GetValue("SRGTPJ:I", typeof(Guid?));
            _a = info.GetString("SRGTPJ:A");
            _p = info.GetInt32("SRGTPJ:P");
            _s = info.GetInt32("SRGTPJ:S");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("SRGTPJ:I", _i);
            info.AddValue("SRGTPJ:A", _a);
            info.AddValue("SRGTPJ:P", _p);
            info.AddValue("SRGTPJ:S", _s);
        }

        private Guid? _i;

        private string _a;

        private int _p;

        private int _s;
    }
}
