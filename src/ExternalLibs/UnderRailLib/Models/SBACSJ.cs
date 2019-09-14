using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("SBACSJ")]
    [Serializable]
    public sealed class SBACSJ : Job
    {
        private SBACSJ(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _i = (Guid?)info.GetValue("SBACSJ:I", typeof(Guid?));
            _a = info.GetString("SBACSJ:A");
            _e = info.GetBoolean("SBACSJ:E");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("SBACSJ:I", _i);
            info.AddValue("SBACSJ:A", _a);
            info.AddValue("SBACSJ:E", _e);
        }

        private bool _e;

        private Guid? _i;

        private string _a;
    }
}
