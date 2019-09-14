using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("ENFEJ")]
    [Serializable]
    public sealed class ENFEJ : Job
    {
        private ENFEJ(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _i = (Guid?)info.GetValue("ENFEJ:I", typeof(Guid?));
            _a = info.GetString("ENFEJ:A");
            _v = info.GetDouble("ENFEJ:V");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("ENFEJ:V", _v);
            info.AddValue("ENFEJ:I", _i);
            info.AddValue("ENFEJ:A", _a);
        }
        
        private Guid? _i;

        private string _a;

        private double _v;
    }
}
