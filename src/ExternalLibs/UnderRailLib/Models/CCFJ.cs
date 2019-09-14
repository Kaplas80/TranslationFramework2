using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Enums;

namespace UnderRailLib.Models
{
    [EncodedTypeName("CCFJ")]
    [Serializable]
    public sealed class CCFJ : Job
    {
        private CCFJ(SerializationInfo A_0, StreamingContext A_1) : base(A_0, A_1)
        {
            _t = (eSPCJBT) A_0.GetValue("CCFJ:T", typeof(eSPCJBT));
            _ct = (A_0.GetValue("CCFJ:CT", typeof(Guid?)) as Guid?);
            _f = A_0.GetString("CCFJ:F");
            _p = A_0.GetBoolean("CCFJ:P");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("CCFJ:T", _t);
            info.AddValue("CCFJ:CT", _ct);
            info.AddValue("CCFJ:F", _f);
            info.AddValue("CCFJ:P", _p);
        }

        private Guid? _ct;

        private eSPCJBT _t = eSPCJBT.c;

        private string _f;

        private bool _p;
    }
}
