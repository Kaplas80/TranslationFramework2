using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("R04")]
    [Serializable]
    public sealed class R04 : Condition
    {
        private R04(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _m = info.GetInt32("R04:M");
            _x = info.GetInt32("R04:X");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("R04:M", _m);
            info.AddValue("R04:X", _x);
        }

        private int _m;

        private int _x;
    }
}
