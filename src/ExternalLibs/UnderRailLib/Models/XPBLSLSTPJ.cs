using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("XPBLSLSTPJ")]
    [Serializable]
    public sealed class XPBLSLSTPJ : Job
    {
        private XPBLSLSTPJ(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _t = info.GetInt32("XPBLSLSTPJ:T");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("XPBLSLSTPJ:T", _t);
        }

        private int _t;
    }
}
