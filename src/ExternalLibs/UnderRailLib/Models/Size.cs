using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("S")]
    [Serializable]
    public struct Size : ISerializable
    {
        private Size(SerializationInfo info, StreamingContext ctx)
        {
            if (DataModelVersion.MajorVersion >= 6)
            {
                _width = info.GetInt32("W");
                _height = info.GetInt32("H");
                return;
            }
            _width = info.GetInt32("_Width");
            _height = info.GetInt32("_Height");
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("W", _width);
            info.AddValue("H", _height);
        }

        public int _width;

        public int _height;
    }
}
