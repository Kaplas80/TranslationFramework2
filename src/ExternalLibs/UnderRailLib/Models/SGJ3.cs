using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("SGJ3")]
    [Serializable]
    public sealed class SGJ3 : Job
    {
        private SGJ3(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _g = info.GetString("SGJ3:G");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("SGJ3:G", _g);
        }

        private string _g;
    }
}
