using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("XPBLRFTVK")]
    [Serializable]
    public sealed class XPBLRFTVK : Job
    {
        private XPBLRFTVK(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _tn = info.GetInt32("XPBLRFTVK:TN");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("XPBLRFTVK:TN", _tn);
        }

        private int _tn;
    }
}
