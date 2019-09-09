using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("A4")]
    [Serializable]
    public class Actor : ISerializable
    {
        protected Actor(SerializationInfo info, StreamingContext ctx)
        {
            if (DataModelVersion.MajorVersion != 0)
            {
                _name = info.GetString("A4:N");
                _picture = info.GetString("A4:P");
                return;
            }
            if (GetType() == typeof(Actor))
            {
                _name = info.GetString("_Name");
                _picture = info.GetString("_Picture");
                return;
            }
            _name = info.GetString("Actor+_Name");
            _picture = info.GetString("Actor+_Picture");
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            info.AddValue("A4:N", _name);
            info.AddValue("A4:P", _picture);
        }
        
        private string _name;
        private string _picture;
    }
}
