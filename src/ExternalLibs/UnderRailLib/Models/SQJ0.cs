using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("SQJ0")]
    [Serializable]
    public sealed class SQJ0 : Job
    {
        private SQJ0(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _qn = info.GetString("SQJ0:QN");
            _pe = info.GetBoolean("SQJ0:PE");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("SQJ0:QN", _qn);
            info.AddValue("SQJ0:PE", _pe);
        }

        private bool _pe = true;

        private string _qn;
    }
}
