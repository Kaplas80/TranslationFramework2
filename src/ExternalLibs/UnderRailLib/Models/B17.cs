using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("B17")]
    [Serializable]
    public sealed class B17 : ItemGeneratorBase
    {
        private B17(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MinorVersion >= 264)
            {
                _l = info.GetBoolean("B17:L");
                _m = info.GetBoolean("B17:M");
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("B17:L", _l);
            info.AddValue("B17:M", _m);
        }

        private bool _l;

        private bool _m;
    }
}
