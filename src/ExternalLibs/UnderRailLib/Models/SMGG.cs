using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.TimelapseVertigo.Rules.Items;

namespace UnderRailLib.Models
{
    [EncodedTypeName("SMGG")]
    [Serializable]
    public sealed class SMGG : WCGB
    {
        private SMGG(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _b = (info.GetValue("SMGG:B", typeof(BarrelType?)) as BarrelType?);
            _f = info.GetString("SMGG:F");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("SMGG:B", _b);
            info.AddValue("SMGG:F", _f);
        }

        private string _f;

        private BarrelType? _b;
    }
}
