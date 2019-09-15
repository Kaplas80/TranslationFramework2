using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("IFX")]
    [Serializable]
    public abstract class IFX : ISerializable
    {
        protected IFX(SerializationInfo info, StreamingContext ctx)
        {
            a = info.GetInt32("IFX:R");
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            info.AddValue("IFX:R", a);
        }

        private int a;
    }
}
