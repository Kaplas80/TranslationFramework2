using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("CMCHPC")]
    [Serializable]
    public sealed class CMCHPC : Condition
    {
        private CMCHPC(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _v = info.GetSingle("CMCHPC:V");
            _i = info.GetBoolean("CMCHPC:I");
            _n = (info.GetValue("CMCHPC:N", typeof(Guid?)) as Guid?);
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("CMCHPC:V", _v);
            info.AddValue("CMCHPC:I", _i);
            info.AddValue("CMCHPC:N", _n);
        }

        private bool _i;

        private float _v;

        private Guid? _n;
    }
}
