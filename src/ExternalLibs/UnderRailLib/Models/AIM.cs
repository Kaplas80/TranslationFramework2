using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("AIM")]
    [Serializable]
    public abstract class AIM : ISerializable
    {
        protected AIM(SerializationInfo info, StreamingContext ctx)
        {
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
        }
    }
}
