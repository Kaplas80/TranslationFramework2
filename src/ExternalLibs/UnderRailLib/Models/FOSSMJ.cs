using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("FOSSMJ")]
    [Serializable]
    public sealed class FOSSMJ : Job
    {
        private FOSSMJ(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _a = info.GetInt32("FOSSMJ:A");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("FOSSMJ:A", _a);
        }

        private int _a;
    }
}
