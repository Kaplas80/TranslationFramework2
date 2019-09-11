using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("CKG")]
    [Serializable]
    public sealed class CKG : WCGB, ISerializable
    {
        private CKG(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MinorVersion >= 224)
            {
                _m = info.GetString("CKG:M");
                _o = info.GetString("CKG:O");
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("CKG:M", _m);
            info.AddValue("CKG:O", _o);
        }

        private string _m;

        private string _o;
    }
}
