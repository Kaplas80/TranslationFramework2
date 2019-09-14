using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("QDEJ")]
    [Serializable]
    public sealed class QDEJ : Job
    {
        private QDEJ(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _e = (info.GetValue("QDEJ:E", typeof(DLE)) as DLE);
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("QDEJ:E", _e);
        }

        private DLE _e;
    }
}