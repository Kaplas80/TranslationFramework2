using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("QPSRJ")]
    [Serializable]
    public sealed class QPSRJ : Job
    {
        private QPSRJ(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _id = (Guid)info.GetValue("QPSRJ:I", typeof(Guid));
            _d = info.GetInt32("QPSRJ:D");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("QPSRJ:I", _id);
            info.AddValue("QPSRJ:D", _d);
        }

        private Guid _id;

        private int _d;
    }
}
