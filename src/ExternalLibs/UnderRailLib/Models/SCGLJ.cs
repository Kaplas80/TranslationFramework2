using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("SCGLJ")]
    [Serializable]
    public sealed class SCGLJ : Job
    {
        private SCGLJ(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _i = (Guid?) info.GetValue("SCGLJ:I", typeof(Guid?));
            _a = info.GetString("SCGLJ:A");
            _l = (info.GetValue("SCGLJ:L", typeof(SpatialPointer)) as SpatialPointer);
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("SCGLJ:I", _i);
            info.AddValue("SCGLJ:A", _a);
            info.AddValue("SCGLJ:L", _l);
        }

        private SpatialPointer _l;

        private Guid? _i;

        private string _a;
    }
}
