using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("L52")]
    [Serializable]
    public sealed class L52 : A50, ISerializable
    {
        private L52(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _l = info.GetString("L52:L");
            _b = info.GetString("L52:B");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("L52:L", _l);
            info.AddValue("L52:B", _b);
        }

        private string _l;

        private string _b;
    }
}
