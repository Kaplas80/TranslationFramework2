using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("CGG")]
    [Serializable]
    public sealed class CGG : WCGB, ISerializable
    {
        private CGG(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MinorVersion >= 224)
            {
                _l = info.GetBoolean("CGG:L");
                _m = info.GetBoolean("CGG:M");
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("CGG:L", _l);
            info.AddValue("CGG:M", _m);
        }

        private bool _l;

        private bool _m;
    }
}
