using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("TCHSRHCS")]
    [Serializable]
    public sealed class TCHSRHCS : Job
    {
        private TCHSRHCS(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _o = info.GetBoolean("TCHSRHCS:O");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("TCHSRHCS:O", _o);
        }

        private bool _o;
    }
}
