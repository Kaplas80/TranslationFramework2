using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("EC")]
    [Serializable]
    public sealed class EffectContext : ISerializable
    {
        private EffectContext(SerializationInfo info, StreamingContext ctx)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                _character = (C1)info.GetValue("EC:C", typeof(C1));
                return;
            }
            if (GetType() == typeof(EffectContext))
            {
                _character = (C1)info.GetValue("_Character", typeof(C1));
                return;
            }
            _character = (C1)info.GetValue("EffectContext+_Character", typeof(C1));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            info.AddValue("EC:C", _character);
        }

        private C1 _character;
    }
}
