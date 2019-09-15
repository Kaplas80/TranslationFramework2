using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("SNF")]
    [Serializable]
    public sealed class SNF : ISerializable
    {
        private SNF(SerializationInfo info, StreamingContext ctx)
        {
            _value = info.GetInt32("SNF:V");
            _modifiedValue = info.GetInt32("SNF:M");
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            info.AddValue("SNF:V", _value);
            info.AddValue("SNF:M", _modifiedValue);
        }

        private int _value;

        private int _modifiedValue;
    }
}
