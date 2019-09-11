using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("SG5")]
    [Serializable]
    public sealed class SG5 : WCGB
    {
        private SG5(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _f = info.GetString("SG5:F");
            if (DataModelVersion.MinorVersion >= 408)
            {
                _c = (info.GetValue("SG5:C", typeof(bool?)) as bool?);
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("SG5:F", _f);
            info.AddValue("SG5:C", _c);
        }

        private string _f;

        private bool? _c;
    }
}
