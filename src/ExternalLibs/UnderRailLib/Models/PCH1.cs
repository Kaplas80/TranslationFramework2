using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("PCH1")]
    [Serializable]
    public sealed class PCH1 : Condition
    {
        private PCH1(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _m = info.GetSingle("PCH1:M");
            _x = info.GetSingle("PCH1:X");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("PCH1:M", _m);
            info.AddValue("PCH1:X", _x);
        }

        private float _m;
        private float _x = 1f;
    }
}
