using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("ESI")]
    [Serializable]
    internal sealed class EventSerializationInfo : ISerializable
    {
        public EventSerializationInfo()
        {
        }

        private EventSerializationInfo(SerializationInfo info, StreamingContext ctx)
        {
            SerializationHelper.ReadList<DelegateInfo>("ESI:S", ref DelegateInfoList, info);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            SerializationHelper.WriteList<DelegateInfo>("ESI:S", DelegateInfoList, info);
        }

        public List<DelegateInfo> DelegateInfoList = new List<DelegateInfo>(); //a
    }
}
