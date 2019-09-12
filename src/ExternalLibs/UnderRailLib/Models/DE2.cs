using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("DE2")]
    [Serializable]
    public abstract class DE2 : ISerializable
    {
        protected DE2(SerializationInfo info, StreamingContext ctx)
        {
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
        }
    }
}
