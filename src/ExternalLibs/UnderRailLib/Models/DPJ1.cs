using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("DPJ1")]
    [Serializable]
    public sealed class DPJ1 : Job
    {
        private DPJ1(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _pv = info.GetInt32("DPJ1:PV");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("DPJ1:PV", _pv);
        }

        private int _pv;
    }
}
