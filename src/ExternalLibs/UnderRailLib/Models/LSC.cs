using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("LSC")]
    [Serializable]
    public sealed class LSC : Condition
    {
        private LSC(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _a = info.GetString("LSC:A");
            _e = (info.GetValue("LSC:E", typeof(Guid?)) as Guid?);
            _s = info.GetBoolean("LSC:S");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("LSC:A", _a);
            info.AddValue("LSC:E", _e);
            info.AddValue("LSC:S", _s);
        }

        private string _a;

        private Guid? _e;

        private bool _s;
    }
}
