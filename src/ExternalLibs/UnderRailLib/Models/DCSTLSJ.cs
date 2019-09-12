using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("DCSTLSJ")]
    [Serializable]
    public sealed class DCSTLSJ : Job
    {
        private DCSTLSJ(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _e = info.GetBoolean("DCSTLSJ:E");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("DCSTLSJ:E", _e);
        }

        private bool _e;
    }
}
