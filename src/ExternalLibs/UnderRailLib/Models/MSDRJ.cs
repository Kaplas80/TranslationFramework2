using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("MSDRJ")]
    [Serializable]
    public sealed class MSDRJ : Job
    {
        private MSDRJ(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _i = (Guid)info.GetValue("MSDRJ:I", typeof(Guid));
            _l = (info.GetValue("MSDRJ:L", typeof(SpatialPointer)) as SpatialPointer);
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("MSDRJ:I", _i);
            info.AddValue("MSDRJ:L", _l);
        }

        private Guid _i;

        private SpatialPointer _l;
    }
}
