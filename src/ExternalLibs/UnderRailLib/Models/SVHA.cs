using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("SVHA")]
    [Serializable]
    public sealed class SVHA : BaseAction
    {
        private SVHA(SerializationInfo A_0, StreamingContext A_1) : base(A_0, A_1)
        {
            _c = A_0.GetString("SVHA:C");
            _d = A_0.GetInt32("SVHA:D");
            _f = A_0.GetString("SVHA:F");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("SVHA:C", _c);
            info.AddValue("SVHA:D", _d);
            info.AddValue("SVHA:F", _f);
        }

        private string _c;

        private int _d;

        private string _f;
    }
}
