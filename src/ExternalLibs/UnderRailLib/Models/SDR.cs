using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("SDR")]
    [Serializable]
    public sealed class SDR : ISerializable
    {
        private SDR(SerializationInfo info, StreamingContext ctx)
        {
            _l = (SpatialPointer)info.GetValue("SDR:L", typeof(SpatialPointer));
            _c = (Guid)info.GetValue("SDR:C", typeof(Guid));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            info.AddValue("SDR:L", _l);
            info.AddValue("SDR:C", _c);
        }

        public SpatialPointer _l;

        public Guid _c;
    }
}
