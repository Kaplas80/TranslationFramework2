using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("PIIAWPC")]
    [Serializable]
    public sealed class PIIAWPC : Condition
    {
        private PIIAWPC(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _p = info.GetString("PIIAWPC:P");
            _i = info.GetBoolean("PIIAWPC:I");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("PIIAWPC:P", _p);
            info.AddValue("PIIAWPC:I", _i);
        }

        private string _p;

        private bool _i;
    }
}
