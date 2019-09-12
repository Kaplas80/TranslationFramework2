using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("WAM")]
    [Serializable]
    public abstract class WAM : ISerializable
    {
        protected WAM(SerializationInfo info, StreamingContext context)
        {
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
        }
    }
}
