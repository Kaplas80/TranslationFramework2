using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Interfaces;

namespace UnderRailLib.Models
{
    [EncodedTypeName("RV0")]
    [Serializable]
    public sealed class RV0 : AIM, iD0
    {
        private RV0(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _c = info.GetDouble("RV0:C");
            _h = info.GetInt32("RV0:H");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("RV0:C", _c);
            info.AddValue("RV0:H", _h);
        }

        private double _c;

        private int _h;
    }
}
