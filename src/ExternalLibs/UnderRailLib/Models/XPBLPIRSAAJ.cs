using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("XPBLPIRSAAJ")]
    [Serializable]
    public sealed class XPBLPIRSAAJ : Job
    {
        private XPBLPIRSAAJ(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _o = info.GetString("XPBLPIRSAAJ:O");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("XPBLPIRSAAJ:O", _o);
        }

        private string _o;
    }
}
