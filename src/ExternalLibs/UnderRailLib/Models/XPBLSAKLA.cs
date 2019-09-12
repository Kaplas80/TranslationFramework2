using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("XPBLSAKLA")]
    [Serializable]
    public sealed class XPBLSAKLA : BaseAction
    {
        private XPBLSAKLA(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _l = info.GetInt32("XPBLSAKLA:L");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("XPBLSAKLA:L", _l);
        }

        private int _l;
    }
}
