using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("RDEJ")]
    [Serializable]
    public sealed class RDEJ : Job
    {
        private RDEJ(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _c = info.GetString("RDEJ:C");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("RDEJ:C", _c);
        }

        private string _c;
    }
}
