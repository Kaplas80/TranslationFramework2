using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("CVHC")]
    [Serializable]
    public abstract class CVHC : ISerializable
    {
        protected CVHC(SerializationInfo info, StreamingContext ctx)
        {
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
        }
    }
}
