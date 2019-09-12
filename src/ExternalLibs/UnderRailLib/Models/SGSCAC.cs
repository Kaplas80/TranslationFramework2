using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("SGSCAC")]
    [Serializable]
    public sealed class SGSCAC : Condition
    {
        private SGSCAC(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _c = info.GetString("SGSCAC:C");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("SGSCAC:C", _c);
        }

        private string _c;
    }
}
