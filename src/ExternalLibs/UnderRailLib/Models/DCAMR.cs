using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("DCAMR")]
    [Serializable]
    public sealed class DCAMR : BaseAction
    {
        private DCAMR(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _r = info.GetInt32("DCAMR:R");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("DCAMR:R", _r);
        }

        private int _r;
    }
}
