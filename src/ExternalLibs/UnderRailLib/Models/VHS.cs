using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("VHS")]
    [Serializable]
    public sealed class VHS : ISerializable
    {
        private VHS(SerializationInfo info, StreamingContext ctx)
        {
            _r = info.GetString("VHS:R");
            _s = info.GetString("VHS:S");
            _v = info.GetSingle("VHS:V");
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            info.AddValue("VHS:R", _r);
            info.AddValue("VHS:S", _s);
            info.AddValue("VHS:V", _v);
        }

        public string _r;

        public string _s;

        public float _v = 1f;
    }
}
