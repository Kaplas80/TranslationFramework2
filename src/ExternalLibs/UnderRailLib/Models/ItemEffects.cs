using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("IFX")]
    [Serializable]
    public abstract class ItemEffects : ISerializable
    {
        protected ItemEffects(SerializationInfo A_0, StreamingContext A_1)
        {
            _remainingTicks = A_0.GetInt32("IFX:R");
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            info.AddValue("IFX:R", _remainingTicks);
        }

        private int _remainingTicks;
    }
}
