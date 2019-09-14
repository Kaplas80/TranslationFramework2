using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("IESM")]
    [Serializable]
    public sealed class IESM : WAM
    {
        private IESM(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _value = info.GetDouble("IESM:V");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("IESM:V", _value);
        }

        private double _value;
    }
}
