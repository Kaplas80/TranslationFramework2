using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("ZTJ")]
    [Serializable]
    public sealed class ZTJ : Job
    {
        private ZTJ(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _d = (info.GetValue("ZTJ:D", typeof(LocaleReference)) as LocaleReference);
            if (DataModelVersion.MinorVersion >= 216)
            {
                _s = info.GetInt32("ZTJ:S");
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("ZTJ:D", _d);
            info.AddValue("ZTJ:S", _s);
        }

        private LocaleReference _d = new LocaleReference();

        private int _s;
    }
}
