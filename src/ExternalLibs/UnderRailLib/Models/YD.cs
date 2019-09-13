using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("YD")]
    [Serializable]
    public sealed class YD : Job
    {
        private YD(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _i = (Guid?)info.GetValue("YD:I", typeof(Guid?));
            _a = info.GetString("YD:A");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("YD:I", _i);
            info.AddValue("YD:A", _a);
        }

        private Guid? _i;

        private string _a;
    }
}
