using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("STFM")]
    [Serializable]
    public sealed class STFM : Job
    {
        private STFM(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _i = (Guid?)info.GetValue("STFM:I", typeof(Guid?));
            _a = info.GetString("STFM:A");
            _m = (info.GetValue("STFM:M", typeof(CS)) as CS);
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("STFM:I", _i);
            info.AddValue("STFM:A", _a);
            info.AddValue("STFM:M", _m);
        }

        private CS _m;

        private Guid? _i;

        private string _a;
    }
}
