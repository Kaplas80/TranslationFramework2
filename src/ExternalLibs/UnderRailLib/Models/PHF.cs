using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("PHF")]
    [Serializable]
    public sealed class PHF : Condition
    {
        private PHF(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _f = info.GetString("PHF:F");
            if (DataModelVersion.MinorVersion >= 474)
            {
                _i = info.GetBoolean("PHF:I");
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("PHF:F", _f);
            if (DataModelVersion.MinorVersion >= 474)
            {
                info.AddValue("PHF:I", _i);
            }
        }

        private string _f;

        private bool _i;
    }
}