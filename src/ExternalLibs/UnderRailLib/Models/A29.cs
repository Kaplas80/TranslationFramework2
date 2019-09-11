using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.TimelapseVertigo.Rules.Items;

namespace UnderRailLib.Models
{
    [EncodedTypeName("A29")]
    [Serializable]
    public sealed class A29 : WCGB
    {
        private A29(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _b = (info.GetValue("A29:B", typeof(BarrelType?)) as BarrelType?);
            _f = info.GetString("A29:F");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("A29:B", _b);
            info.AddValue("A29:F", _f);
        }

        private string _f;

        private BarrelType? _b;
    }
}
