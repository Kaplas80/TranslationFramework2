using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("CCC")]
    [Serializable]
    public abstract class CharacterCombatComponent : ISerializable
    {
        protected CharacterCombatComponent(SerializationInfo A_0, StreamingContext A_1)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                _character = (C1)A_0.GetValue("CCC:C", typeof(C1));
                return;
            }
            if (GetType() == typeof(CharacterCombatComponent))
            {
                _character = (C1)A_0.GetValue("_Character", typeof(C1));
                return;
            }
            _character = (C1)A_0.GetValue("CharacterCombatComponent+_Character", typeof(C1));
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            info.AddValue("CCC:C", _character);
        }

        private C1 _character;
    }
}
