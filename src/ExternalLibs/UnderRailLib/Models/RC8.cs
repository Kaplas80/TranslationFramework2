using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("RC8")]
    [Serializable]
    public sealed class RC8 : Condition
    {
        private RC8(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _c = info.GetDouble("RC8:C");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("RC8:C", _c);
        }

        private double _c;
    }
}
