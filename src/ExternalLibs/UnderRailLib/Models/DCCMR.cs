using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("DCCMR")]
    [Serializable]
    public sealed class DCCMR : BaseAction
    {
        private DCCMR(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _id = (Guid) info.GetValue("DCCMR:I", typeof(Guid));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("DCCMR:I", _id);
        }

        private Guid _id;
    }
}
