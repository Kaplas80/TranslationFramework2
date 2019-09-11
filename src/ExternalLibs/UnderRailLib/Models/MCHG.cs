using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("MCHG")]
    [Serializable]
    public sealed class MCHG : WCGB, ISerializable
    {
        private MCHG(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MinorVersion >= 224)
            {
                _m = info.GetString("MCHG:M");
                _o = info.GetString("MCHG:O");
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("MCHG:M", _m);
            info.AddValue("MCHG:O", _o);
        }

        private string _m;

        private string _o;
    }
}
