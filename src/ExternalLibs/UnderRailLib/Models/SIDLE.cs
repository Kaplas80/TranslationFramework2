using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("SIDLE")]
    [Serializable]
    public sealed class SIDLE : LNCDLE
    {
        private SIDLE(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _a = info.GetInt32("SIDLE:A");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("SIDLE:A", _a);
        }

        private int _a;
    }
}
