using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("SEAOJ")]
    [Serializable]
    public sealed class SEAOJ : Job
    {
        private SEAOJ(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _i = (Guid?)info.GetValue("SEAOJ:I", typeof(Guid?));
            _a = info.GetString("SEAOJ:A");
            _l = (info.GetValue("SEAOJ:L", typeof(float?)) as float?);
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("SEAOJ:I", _i);
            info.AddValue("SEAOJ:A", _a);
            info.AddValue("SEAOJ:L", _l);
        }

        private float? _l;

        private Guid? _i;

        private string _a;
    }
}
