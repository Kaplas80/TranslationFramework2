using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("SLAPJ")]
    [Serializable]
    public sealed class SLAPJ : Job
    {
        private SLAPJ(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _i = info.GetValue("SLAPJ:I", typeof(Guid?)) as Guid?;
            _a = info.GetString("SLAPJ:A");
            _d = info.GetString("SLAPJ:D");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("SLAPJ:I", _i);
            info.AddValue("SLAPJ:A", _a);
            info.AddValue("SLAPJ:D", _d);
        }

        private Guid? _i;
        private string _a;
        private string _d;
    }
}
