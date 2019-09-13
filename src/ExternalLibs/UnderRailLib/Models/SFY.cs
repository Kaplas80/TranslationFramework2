using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.TimelapseVertigo.Rules.Combat;

namespace UnderRailLib.Models
{
    [EncodedTypeName("SFY")]
    [Serializable]
    public struct SFY : ISerializable
    {
        private SFY(SerializationInfo info, StreamingContext ctx)
        {
            _i = (ImpactSpeed) info.GetValue("SFY:I", typeof(ImpactSpeed));
            _b = info.GetInt32("SFY:B");
            _n = info.GetDouble("SFY:N");
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("SFY:I", _i);
            info.AddValue("SFY:B", _b);
            info.AddValue("SFY:N", _n);
        }

        private ImpactSpeed _i;

        private int _b;

        private double _n;
    }
}
