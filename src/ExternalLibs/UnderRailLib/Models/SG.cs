using System;
using System.Runtime.Serialization;
using TimelapseVertigo.Rules.Items;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.TimelapseVertigo.Rules.Items;

namespace UnderRailLib.Models
{
    [EncodedTypeName("SG")]
    [Serializable]
    public sealed class SG : WCGB
    {
        private SG(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _b = (info.GetValue("SG:B", typeof(BarrelType?)) as BarrelType?);
            _f = info.GetString("SG:F");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("SG:B", _b);
            info.AddValue("SG:F", _f);
        }

        private string _f;
        private BarrelType? _b;
    }
}
