using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Interfaces;

namespace UnderRailLib.Models
{
    [EncodedTypeName("Q1")]
    [Serializable]
    public sealed class Q1 : Job
    {
        private Q1(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _l = info.GetString("Q1:L");
            _r = info.GetInt32("Q1:R");
            _p = (info.GetValue("Q1:P", typeof(iLCPP)) as iLCPP);
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("Q1:L", _l);
            info.AddValue("Q1:R", _r);
            info.AddValue("Q1:P", _p);
        }

        private string _l;
        private int _r;

        private iLCPP _p;
    }
}
