using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("TTRJ")]
    [Serializable]
    public sealed class TTRJ : Job
    {
        private TTRJ(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _l = info.GetString("TTRJ:L");
            _r = info.GetString("TTRJ:R");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("TTRJ:L", _l);
            info.AddValue("TTRJ:R", _r);
        }

        private string _l;

        private string _r;
    }
}
