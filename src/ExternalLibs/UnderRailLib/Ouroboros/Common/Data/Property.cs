using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Ouroboros.Common.Data
{
    [EncodedTypeName("P")]
    [Serializable]
    public sealed class Property : ISerializable
    {
        private Property(SerializationInfo info, StreamingContext ctx)
        {
            if (DataModelVersion.MajorVersion != 0)
            {
                _name = info.GetString("P:N");
                _value = info.GetValue("P:V", typeof(object));
                return;
            }

            if (GetType() == typeof(Property))
            {
                _name = info.GetString("_Name");
                _value = info.GetValue("_Value", typeof(object));
                return;
            }

            _name = info.GetString("Property+_Name");
            _value = info.GetValue("Property+_Value", typeof(object));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            info.AddValue("P:N", _name);
            info.AddValue("P:V", _value);
        }

        private string _name;

        private object _value;
    }
}

