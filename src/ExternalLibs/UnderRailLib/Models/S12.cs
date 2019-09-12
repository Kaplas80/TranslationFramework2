using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("S12")]
    [Serializable]
    public sealed class S12 : Job
    {
        private S12(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _i = info.GetValue("S12:I", typeof(Guid?)) as Guid?;
            _s = info.GetBoolean("S12:S");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("S12:I", _i);
            info.AddValue("S12:S", _s);
        }

        private Guid? _i;
        private bool _s;
    }
}
