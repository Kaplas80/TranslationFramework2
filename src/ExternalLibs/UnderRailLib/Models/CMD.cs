using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("CMD")]
    [Serializable]
    public sealed class CMD : ISerializable
    {
        private CMD(SerializationInfo info, StreamingContext ctx)
        {
            _n = info.GetString("CMD:N");
            _d = (info.GetValue("CMD:D", typeof(double?)) as double?);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            info.AddValue("CMD:N", _n);
            info.AddValue("CMD:D", _d);
        }

        public string _n;

        public double? _d;
    }
}
