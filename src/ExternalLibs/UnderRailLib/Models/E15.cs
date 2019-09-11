using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("E15")]
    [Serializable]
    public sealed class E15 : WCGB, ISerializable
    {
        private E15(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _p = info.GetString("E15:P");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("E15:P", _p);
        }

        private string _p;
    }
}
