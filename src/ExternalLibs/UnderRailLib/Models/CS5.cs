using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("CS5")]
    [Serializable]
    public sealed class CS5 : ISerializable
    {
        private CS5(SerializationInfo info, StreamingContext ctx)
        {
            _r = info.GetDouble("CS5:R");
            _d = info.GetDouble("CS5:D");
            if (DataModelVersion.MinorVersion >= 548)
            {
                _a = info.GetBoolean("CS5:A");
                _s = info.GetBoolean("CS5:S");
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            info.AddValue("CS5:R", _r);
            info.AddValue("CS5:D", _d);
            info.AddValue("CS5:A", _a);
            info.AddValue("CS5:S", _s);
        }

        public double _r;

        public double _d;

        public bool _a;

        public bool _s;
    }
}
