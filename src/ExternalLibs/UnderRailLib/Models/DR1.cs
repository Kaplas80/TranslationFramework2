using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("DR1")]
    [Serializable]
    public struct DR1 : ISerializable
    {
        private DR1(SerializationInfo info, StreamingContext ctx)
        {
            if (DataModelVersion.MajorVersion >= 6)
            {
                _min = info.GetDouble("MI");
                _max = info.GetDouble("MX");
                return;
            }

            _min = info.GetDouble("Min");
            _max = info.GetDouble("Max");
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("MI", _min);
            info.AddValue("MX", _max);
        }

        public double _min;

        public double _max;
    }
}
