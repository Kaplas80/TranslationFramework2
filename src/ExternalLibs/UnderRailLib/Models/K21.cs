using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("K21")]
    [Serializable]
    public sealed class K21 : DRAIM
    {
        private K21(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _bonus = info.GetDouble("K21:B");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("K21:B", _bonus);
        }

        private double _bonus;
    }
}
