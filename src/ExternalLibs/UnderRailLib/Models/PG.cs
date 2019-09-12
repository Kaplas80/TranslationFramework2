using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.TimelapseVertigo.Rules.Items;

namespace UnderRailLib.Models
{
    [EncodedTypeName("PG")]
    [Serializable]
    public sealed class PG : WCGB
    {
        private PG(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MinorVersion >= 67)
            {
                _b = (info.GetValue("PG:B", typeof(BarrelType?)) as BarrelType?);
                _f = info.GetString("PG:F");
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("PG:B", _b);
            info.AddValue("PG:F", _f);
        }

        private string _f;
        private BarrelType? _b;
    }
}
