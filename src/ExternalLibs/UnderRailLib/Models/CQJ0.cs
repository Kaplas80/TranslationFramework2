using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("CQJ0")]
    [Serializable]
    public sealed class CQJ0 : Job
    {
        private CQJ0(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _qn = info.GetString("CQJ0:QN");
            _pe = info.GetBoolean("CQJ0:PE");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("CQJ0:QN", _qn);
            info.AddValue("CQJ0:PE", _pe);
        }

        private bool _pe = true;

        private string _qn;
    }
}
