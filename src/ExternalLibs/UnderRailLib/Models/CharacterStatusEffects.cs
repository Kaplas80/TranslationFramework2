using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.EventArgs;

namespace UnderRailLib.Models
{
    [EncodedTypeName("CSE")]
    [Serializable]
    public sealed class CharacterStatusEffects : ISerializable
    {
        private CharacterStatusEffects(SerializationInfo info, StreamingContext ctx)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                _character = (C1) info.GetValue("CSE:C", typeof(C1));
                _context = (EffectContext) info.GetValue("CSE:C1", typeof(EffectContext));
                SerializationHelper.ReadList("CSE:SE", ref _statusEffects, info);
                SerializationHelper.ReadList("CSE:ISE", ref _incapacitationStatusEffects, info);
                SerializationHelper.ReadList("CSE:VE", ref _visualEffects, info);
                if (DataModelVersion.MajorVersion >= 5)
                {
                    SerializationHelper.ReadEvent("CSE:SER", ref _statusEffectRemoved, info);
                    SerializationHelper.ReadEvent("CSE:SET", ref _statusEffectTick, info);
                    SerializationHelper.ReadEvent("CSE:SEE", ref _statusEffectExpired, info);
                    SerializationHelper.ReadEvent("CSE:SEA", ref _statusEffectApplied, info);
                    SerializationHelper.ReadEvent("CSE:SESR", ref _statusEffectSoftRemoved, info);
                    return;
                }

                _statusEffectRemoved = (EventHandler<eaSEEA>) info.GetValue("CSE:SER", typeof(EventHandler<eaSEEA>));
                _statusEffectTick = (EventHandler<eaSEEA>) info.GetValue("CSE:SET", typeof(EventHandler<eaSEEA>));
                _statusEffectExpired = (EventHandler<eaSEEA>) info.GetValue("CSE:SEE", typeof(EventHandler<eaSEEA>));
                _statusEffectApplied = (EventHandler<eaSEEA>) info.GetValue("CSE:SEA", typeof(EventHandler<eaSEEA>));
                _statusEffectSoftRemoved = (EventHandler<eaSEEA>) info.GetValue("CSE:SESR", typeof(EventHandler<eaSEEA>));
            }
            else
            {
                if (GetType() == typeof(CharacterStatusEffects))
                {
                    _character = (C1) info.GetValue("_Character", typeof(C1));
                    _context = (EffectContext) info.GetValue("_Context", typeof(EffectContext));
                    _statusEffects = (List<SE1>) info.GetValue("_StatusEffects", typeof(List<SE1>));
                    _incapacitationStatusEffects = (List<SE1>) info.GetValue("_IncapacitationStatusEffects", typeof(List<SE1>));
                    _visualEffects = (List<VisualEffectReference>) info.GetValue("VisualEffects", typeof(List<VisualEffectReference>));
                    _statusEffectRemoved = (EventHandler<eaSEEA>) info.GetValue("StatusEffectRemoved", typeof(EventHandler<eaSEEA>));
                    _statusEffectTick = (EventHandler<eaSEEA>) info.GetValue("StatusEffectTick", typeof(EventHandler<eaSEEA>));
                    _statusEffectExpired = (EventHandler<eaSEEA>) info.GetValue("StatusEffectExpired", typeof(EventHandler<eaSEEA>));
                    try
                    {
                        _statusEffectApplied = (EventHandler<eaSEEA>) info.GetValue("StatusEffectApplied", typeof(EventHandler<eaSEEA>));
                    }
                    catch
                    {
                    }

                    _statusEffectSoftRemoved = (EventHandler<eaSEEA>) info.GetValue("StatusEffectSoftRemoved", typeof(EventHandler<eaSEEA>));
                    return;
                }

                _character = (C1) info.GetValue("CharacterStatusEffects+_Character", typeof(C1));
                _context = (EffectContext) info.GetValue("CharacterStatusEffects+_Context", typeof(EffectContext));
                _statusEffects = (List<SE1>) info.GetValue("CharacterStatusEffects+_StatusEffects", typeof(List<SE1>));
                _incapacitationStatusEffects = (List<SE1>) info.GetValue("CharacterStatusEffects+_IncapacitationStatusEffects",
                    typeof(List<SE1>));
                _visualEffects = (List<VisualEffectReference>) info.GetValue("CharacterStatusEffects+VisualEffects", typeof(List<VisualEffectReference>));
                _statusEffectRemoved = (EventHandler<eaSEEA>) info.GetValue("CharacterStatusEffects+StatusEffectRemoved",
                    typeof(EventHandler<eaSEEA>));
                _statusEffectTick = (EventHandler<eaSEEA>) info.GetValue("CharacterStatusEffects+StatusEffectTick",
                    typeof(EventHandler<eaSEEA>));
                _statusEffectExpired = (EventHandler<eaSEEA>) info.GetValue("CharacterStatusEffects+StatusEffectExpired",
                    typeof(EventHandler<eaSEEA>));
                try
                {
                    _statusEffectApplied = (EventHandler<eaSEEA>) info.GetValue("CharacterStatusEffects+StatusEffectApplied",
                        typeof(EventHandler<eaSEEA>));
                }
                catch
                {
                }

                _statusEffectSoftRemoved = (EventHandler<eaSEEA>) info.GetValue("CharacterStatusEffects+StatusEffectSoftRemoved",
                    typeof(EventHandler<eaSEEA>));
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            info.AddValue("CSE:C", _character);
            info.AddValue("CSE:C1", _context);
            SerializationHelper.WriteList("CSE:SE", _statusEffects, info);
            SerializationHelper.WriteList("CSE:ISE", _incapacitationStatusEffects, info);
            SerializationHelper.WriteList("CSE:VE", _visualEffects, info);
            SerializationHelper.WriteEvent("CSE:SER", _statusEffectRemoved, info);
            SerializationHelper.WriteEvent("CSE:SET", _statusEffectTick, info);
            SerializationHelper.WriteEvent("CSE:SEE", _statusEffectExpired, info);
            SerializationHelper.WriteEvent("CSE:SEA", _statusEffectApplied, info);
            SerializationHelper.WriteEvent("CSE:SESR", _statusEffectSoftRemoved, info);
        }

        private C1 _character;

        private EffectContext _context;

        private List<SE1> _statusEffects = new List<SE1>();

        private List<SE1> _incapacitationStatusEffects = new List<SE1>();

        public List<VisualEffectReference> _visualEffects = new List<VisualEffectReference>();

        private EventHandler<eaSEEA> _statusEffectRemoved;

        private EventHandler<eaSEEA> _statusEffectTick;

        private EventHandler<eaSEEA> _statusEffectExpired;

        private EventHandler<eaSEEA> _statusEffectApplied;

        private EventHandler<eaSEEA> _statusEffectSoftRemoved;
    }
}
