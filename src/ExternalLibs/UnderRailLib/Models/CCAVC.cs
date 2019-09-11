using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("CCAVC")]
    [Serializable]
    public sealed class CCAVC : Condition
    {
        private CCAVC(SerializationInfo A_0, StreamingContext A_1) : base(A_0, A_1)
        {
            _v = A_0.GetString("CHIEC:V");
            _i = A_0.GetBoolean("CHIEC:I");
            _n = (A_0.GetValue("CHIEC:N", typeof(Guid?)) as Guid?);
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("CHIEC:V", _v);
            info.AddValue("CHIEC:I", _i);
            info.AddValue("CHIEC:N", _n);
        }

        private string _v;

        private bool _i;

        private Guid? _n;
    }
}
