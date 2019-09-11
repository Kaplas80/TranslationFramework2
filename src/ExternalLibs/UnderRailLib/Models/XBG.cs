using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("XBG")]
    [Serializable]
    public sealed class XBG : WCGB
    {
        private XBG(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MinorVersion >= 67)
            {
                _b = info.GetString("XBG:B");
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("XBG:B", _b);
        }

        private string _b;
    }
}
