using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("CM1")]
    [Serializable]
    public sealed class CM1 : ISerializable
    {
        private CM1(SerializationInfo info, StreamingContext ctx)
        {
            SerializationHelper.ReadDictionary("CM1:C", ref _c, info);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            SerializationHelper.WriteDictionary("CM1:C", _c, info);
        }

        private Dictionary<string, CS5> _c = new Dictionary<string, CS5>();
    }
}
