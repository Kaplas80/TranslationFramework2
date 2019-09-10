using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("PS01")]
    [Serializable]
    public sealed class PS01 : Condition
    {
        private PS01(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _a = info.GetBoolean("PS01:I");
            _b = info.GetString("PS01:P");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("PS01:I", _a);
            info.AddValue("PS01:P", _b);
        }

        private bool _a;

        private string _b;
    }
}
