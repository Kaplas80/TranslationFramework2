using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.EventArgs;
using UnderRailLib.Models.Interfaces;

namespace UnderRailLib.Models
{
    [EncodedTypeName("BA3")]
    [Serializable]
    public sealed class BaseAbilities : iCHP, c4w, ISerializable
    {
        private BaseAbilities(SerializationInfo info, StreamingContext ctx)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                SerializationHelper.ReadList("BA3:BA", ref _baseAbilities, info);
                _character = (C1)info.GetValue("BA3:C", typeof(C1));
                if (DataModelVersion.MajorVersion >= 5)
                {
                    SerializationHelper.ReadEvent("BA3:BAVC", ref _baseAbilityValueChanged, info);
                }
                else
                {
                    _baseAbilityValueChanged = (EventHandler<eaPCEA>)info.GetValue("BA3:BAVC", typeof(EventHandler<eaPCEA>));
                }
            }
            else if (GetType() == typeof(BaseAbilities))
            {
                _baseAbilities = (List<BaseAbility>)info.GetValue("_BaseAbilities", typeof(List<BaseAbility>));
                _character = (C1)info.GetValue("_Character", typeof(C1));
                _baseAbilityValueChanged = (EventHandler<eaPCEA>)info.GetValue("BaseAbilityValueChanged", typeof(EventHandler<eaPCEA>));
            }
            else
            {
                _baseAbilities = (List<BaseAbility>)info.GetValue("BaseAbilities+_BaseAbilities", typeof(List<BaseAbility>));
                _character = (C1)info.GetValue("BaseAbilities+_Character", typeof(C1));
                _baseAbilityValueChanged = (EventHandler<eaPCEA>)info.GetValue("BaseAbilities+BaseAbilityValueChanged", typeof(EventHandler<eaPCEA>));
            }

            foreach (var cxi in _baseAbilities.OfType<WisdomBaseAbility>())
            {
                _baseAbilities.Remove(cxi);
                break;
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            SerializationHelper.WriteList("BA3:BA", _baseAbilities, info);
            info.AddValue("BA3:C", _character);
            SerializationHelper.WriteEvent("BA3:BAVC", _baseAbilityValueChanged, info);
        }

        private List<BaseAbility> _baseAbilities = new List<BaseAbility>();

        private C1 _character;

        private EventHandler<eaPCEA> _baseAbilityValueChanged;
    }
}
