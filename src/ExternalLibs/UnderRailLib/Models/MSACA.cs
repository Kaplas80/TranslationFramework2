using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("MSACA")]
    [Serializable]
    public sealed class MSACA : BaseAction
    {
        private MSACA(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _a = info.GetInt32("MSACA:A");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("MSACA:A", _a);
        }

        private int _a;
    }
}
