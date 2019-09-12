using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("SGSAEA")]
    [Serializable]
    public sealed class SGSAEA : BaseAction
    {
        private SGSAEA(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _c = info.GetString("SGSAEA:C");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("SGSAEA:C", _c);
        }

        private string _c;
    }
}
