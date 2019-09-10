using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("DLCIIC")]
    [Serializable]
    public sealed class DLCIIC : Condition
    {
        private DLCIIC(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _a = info.GetString("DLCIIC");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("DLCIIC", _a);
        }

        private string _a;
    }
}