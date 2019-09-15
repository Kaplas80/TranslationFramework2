using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("XMD")]
    [Serializable]
    public sealed class XMD : ISerializable
    {
        private XMD(SerializationInfo info, StreamingContext ctx)
        {
            _d = info.GetDouble("XMD:D");
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            info.AddValue("XMD:D", _d);
        }

        public double _d = 1.0;
    }
}
