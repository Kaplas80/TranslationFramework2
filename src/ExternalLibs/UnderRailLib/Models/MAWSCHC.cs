using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.TimelapseVertigo.Rules.Combat;

namespace UnderRailLib.Models
{
    [EncodedTypeName("MAWSCHC")]
    [Serializable]
    public sealed class MAWSCHC : SE2
    {
        private MAWSCHC(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _s = (WeaponAggregateSubtype)info.GetValue("MAWSCHC:S", typeof(WeaponAggregateSubtype));
            _c = info.GetDouble("MAWSCHC:C");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("MAWSCHC:S", _s);
            info.AddValue("MAWSCHC:C", _c);
        }
    
        private WeaponAggregateSubtype _s;

    	private double _c;
    }
}
