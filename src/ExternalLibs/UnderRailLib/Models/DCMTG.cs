using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("DCMTG")]
    [Serializable]
    public sealed class DCMTG : BaseAction
    {
        private DCMTG(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _id = (Guid) info.GetValue("DCMTG:I", typeof(Guid));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("DCMTG:I", _id);
        }

        private Guid _id;
    }
}
