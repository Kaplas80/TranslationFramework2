using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("EDU0")]
    [Serializable]
    public sealed class EDU0 : Job
    {
        private EDU0(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _i = (Guid?)info.GetValue("EDU0:I", typeof(Guid?));
            _a = info.GetString("EDU0:A");
            _e = info.GetBoolean("EDU0:E");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("EDU0:I", _i);
            info.AddValue("EDU0:A", _a);
            info.AddValue("EDU0:E", _e);
        }

        private bool _e;

        private Guid? _i;

        private string _a;
    }
}
