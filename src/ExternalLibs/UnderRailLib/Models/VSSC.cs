using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("VSSC")]
    [Serializable]
    public sealed class VSSC : Condition
    {
        private VSSC(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _a = info.GetString("VSSC:A");
            _e = (info.GetValue("VSSC:E", typeof(Guid?)) as Guid?);
            _s = info.GetBoolean("VSSC:S");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("VSSC:A", _a);
            info.AddValue("VSSC:E", _e);
            info.AddValue("VSSC:S", _s);
        }

        private string _a;

        private Guid? _e;

        private bool _s;
    }
}