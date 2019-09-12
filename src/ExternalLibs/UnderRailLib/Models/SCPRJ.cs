using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("SCPRJ")]
    [Serializable]
    public sealed class SCPRJ : Job
    {
        private SCPRJ(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _i = (Guid?) info.GetValue("SCPRJ:I", typeof(Guid?));
            _a = info.GetString("SCPRJ:A");
            _p = info.GetString("SCPRJ:P");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("SCPRJ:I", _i);
            info.AddValue("SCPRJ:A", _a);
            info.AddValue("SCPRJ:P", _p);
        }

        private string _p;

        private Guid? _i;

        private string _a;
    }
}
