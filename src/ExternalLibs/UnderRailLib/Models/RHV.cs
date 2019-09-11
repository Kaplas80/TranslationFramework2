using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("RHV")]
    [Serializable]
    public sealed class RHV : BaseAction
    {
        private RHV(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _a = info.GetBoolean("RHV:A");
            _m = info.GetInt32("RHV:M");
            _p = info.GetBoolean("RHV:P");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("RHV:A", _a);
            info.AddValue("RHV:M", _m);
            info.AddValue("RHV:P", _p);
        }

        private bool _a;

        private int _m;

        private bool _p;
    }
}
