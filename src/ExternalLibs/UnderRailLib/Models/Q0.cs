using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("Q0")]
    [Serializable]
    public sealed class Q0 : Job
    {
        private Q0(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _l = info.GetString("Q0:L");
            _r = info.GetInt32("Q0:R");
            _p = (info.GetValue("Q0:P", typeof(iLCPP)) as iLCPP);
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("Q0:L", _l);
            info.AddValue("Q0:R", _r);
            info.AddValue("Q0:P", _p);
        }

        private string _l;
        private int _r;

        private iLCPP _p;
    }
}
