using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("ARSIDLE")]
    [Serializable]
    public sealed class ARSIDLE : LNCDLE
    {
        private ARSIDLE(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _s = info.GetBoolean("ARSIDLE:S");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("ARSIDLE:S", _s);
        }

        private bool _s;
    }
}
