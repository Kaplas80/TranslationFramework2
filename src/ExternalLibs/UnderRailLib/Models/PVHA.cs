using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("PVHA")]
    [Serializable]
    public sealed class PVHA : BaseAction
    {
        private PVHA(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _c = info.GetString("PVHA:C");
            _d = info.GetInt32("PVHA:D");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("PVHA:C", _c);
            info.AddValue("PVHA:D", _d);
        }

        private string _c;

        private int _d;
    }
}
