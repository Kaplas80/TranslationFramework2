using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("CSFJ")]
    [Serializable]
    public sealed class CSFJ : Job
    {
        private CSFJ(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _i = (Guid?)info.GetValue("CSFJ:I", typeof(Guid?));
            _a = info.GetString("CSFJ:A");
            _tf = info.GetString("CSFJ:TF");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("CSFJ:I", _i);
            info.AddValue("CSFJ:A", _a);
            info.AddValue("CSFJ:TF", _tf);
        }

        private Guid? _i;

        private string _a;

        private string _tf;
    }
}
