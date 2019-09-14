using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("CS")]
    [Serializable]
    public class CS : ISerializable
    {
        protected CS(SerializationInfo info, StreamingContext ctx)
        {
            SerializationHelper.ReadList("CS:C", ref _c, info);
            _iv = info.GetBoolean("CS:IV");
            _bm = info.GetInt64("CS:BM");
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            SerializationHelper.WriteList("CS:C", _c, info);
            info.AddValue("CS:IV", _iv);
            info.AddValue("CS:BM", _bm);
        }

        public List<byte> _c = new List<byte>();

        public bool _iv;

        private long _bm;
    }
}
