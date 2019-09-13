using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("TACTNPC")]
    [Serializable]
    public sealed class TACTNPC : Job
    {
        private TACTNPC(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _tid = (Guid?)info.GetValue("TACTNPC:TID", typeof(Guid?));
            _oa = info.GetString("TACTNPC:OA");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("TACTNPC:TID", _tid);
            info.AddValue("TACTNPC:OA", _oa);
        }

        private Guid? _tid;

        private string _oa;
    }
}
