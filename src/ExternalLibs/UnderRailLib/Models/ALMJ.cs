using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("ALMJ")]
    [Serializable]
    public sealed class ALMJ : Job
    {
        private ALMJ(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _a = info.GetString("ALMJ:A");
            _p = info.GetString("ALMJ:P");
            _c = info.GetString("ALMJ:C");
            _n = info.GetString("ALMJ:N");
            SerializationHelper.ReadList("ALMJ:R", ref _r, info);
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("ALMJ:A", _a);
            info.AddValue("ALMJ:P", _p);
            info.AddValue("ALMJ:C", _c);
            info.AddValue("ALMJ:N", _n);
            SerializationHelper.WriteList("ALMJ:R", _r, info);
        }

        private string _a;

        private string _p;

        private string _c;

        private string _n;

        private List<string> _r;
    }
}
