using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Enums;

namespace UnderRailLib.Models
{
    [EncodedTypeName("CASVDJ")]
    [Serializable]
    public sealed class CASVDJ : Job
    {
        private CASVDJ(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _i = (Guid?) info.GetValue("CASVDJ:I", typeof(Guid?));
            _a = info.GetString("CASVDJ:A");
            _o = (SplitGateOperation) info.GetValue("CASVDJ:O", typeof(SplitGateOperation));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("CASVDJ:I", _i);
            info.AddValue("CASVDJ:A", _a);
            info.AddValue("CASVDJ:O", _o);
        }

        private Guid? _i;

        private string _a;

        private SplitGateOperation _o;
    }
}
