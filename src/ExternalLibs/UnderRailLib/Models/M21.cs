using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("M21")]
    [Serializable]
    public sealed class M21 : DRAIM
    {
        private M21(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _bonus = info.GetDouble("M21:B");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("M21:B", _bonus);
        }

        private double _bonus;
    }
}
