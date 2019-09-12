using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("RQX")]
    [Serializable]
    public abstract class RQX : ISerializable
    {
        protected RQX(SerializationInfo info, StreamingContext ctx)
        {
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
        }
    }
}
