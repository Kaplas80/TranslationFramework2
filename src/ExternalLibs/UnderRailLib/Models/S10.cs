using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("S10")]
    [Serializable]
    public sealed class S10 : Job
    {
        private S10(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _n = info.GetString("S10:N");
            _e = info.GetBoolean("S10:E");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("S10:N", _n);
            info.AddValue("S10:E", _e);
        }

        private string _n;
        private bool _e;
    }
}
