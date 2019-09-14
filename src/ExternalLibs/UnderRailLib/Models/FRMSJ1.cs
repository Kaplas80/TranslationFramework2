using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("FRMSJ1")]
    [Serializable]
    public sealed class FRMSJ1 : Job
    {
        private FRMSJ1(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _i = (Guid?)info.GetValue("FRMSJ1:I", typeof(Guid?));
            _a = info.GetString("FRMSJ1:A");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("FRMSJ1:I", _i);
            info.AddValue("FRMSJ1:A", _a);
        }

        private Guid? _i;

        private string _a;
    }
}
