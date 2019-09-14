using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("ASJ")]
    [Serializable]
    public sealed class ASJ : Job
    {
        private ASJ(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _i = (Guid?)info.GetValue("ASJ:I", typeof(Guid?));
            _a = info.GetString("ASJ:A");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("ASJ:I", _i);
            info.AddValue("ASJ:A", _a);
        }
        
        private Guid? _i;

        private string _a;
    }
}
