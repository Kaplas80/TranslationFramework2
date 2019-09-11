using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("CHIEC")]
    [Serializable]
    public sealed class CHIEC : Condition
    {
        private CHIEC(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _ic = info.GetString("CHIEC:IC");
            _i = info.GetBoolean("CHIEC:I");
            _n = (info.GetValue("CHIEC:N", typeof(Guid?)) as Guid?);
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("CHIEC:IC", _ic);
            info.AddValue("CHIEC:I", _i);
            info.AddValue("CHIEC:N", _n);
        }

        private string _ic;

        private bool _i;

        private Guid? _n;
    }
}
