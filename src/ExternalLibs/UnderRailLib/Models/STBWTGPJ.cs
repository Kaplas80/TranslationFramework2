using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("STBWTGPJ")]
    [Serializable]
    public sealed class STBWTGPJ : Job
    {
        private STBWTGPJ(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _p = info.GetString("STBWTGPJ:P");
            _o = (info.GetValue("STBWTGPJ:O", typeof(TimeSpan?)) as TimeSpan?);
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("STBWTGPJ:P", _p);
            info.AddValue("STBWTGPJ:O", _o);
        }

        private string _p;

        private TimeSpan? _o;
    }
}
