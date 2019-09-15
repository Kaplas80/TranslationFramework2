using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.TimelapseVertigo.Rules.Combat;

namespace UnderRailLib.Models
{
    [EncodedTypeName("CPO")]
    [Serializable]
    public sealed class CharacterPsiOffense : CharacterCombatComponent
    {
        private CharacterPsiOffense(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                _skillLevel = info.GetInt32("CPO:SL");
                _criticalDamageBonus = info.GetSingle("CPO:CDB");
                _criticalChance = info.GetSingle("CPO:CC");
                _school = (PsiSchool)info.GetValue("CPO:S", typeof(PsiSchool));
                _interruptionInfo = (InterruptibleAttackInfo)info.GetValue("CPO:II", typeof(InterruptibleAttackInfo));
                if (DataModelVersion.MinorVersion >= 162)
                {
                    _c = info.GetDouble("CPO:C");
                    return;
                }
                _c = 1.0;
            }
            else
            {
                if (GetType() == typeof(CharacterPsiOffense))
                {
                    _skillLevel = info.GetInt32("_SkillLevel");
                    _criticalDamageBonus = info.GetSingle("_CriticalDamageBonus");
                    _criticalChance = info.GetSingle("_CriticalChance");
                    _school = (PsiSchool)info.GetValue("_School", typeof(PsiSchool));
                    _interruptionInfo = (InterruptibleAttackInfo)info.GetValue("_InterruptionInfo", typeof(InterruptibleAttackInfo));
                    return;
                }
                _skillLevel = info.GetInt32("CharacterPsiOffense+_SkillLevel");
                _criticalDamageBonus = info.GetSingle("CharacterPsiOffense+_CriticalDamageBonus");
                _criticalChance = info.GetSingle("CharacterPsiOffense+_CriticalChance");
                _school = (PsiSchool)info.GetValue("CharacterPsiOffense+_School", typeof(PsiSchool));
                _interruptionInfo = (InterruptibleAttackInfo)info.GetValue("CharacterPsiOffense+_InterruptionInfo", typeof(InterruptibleAttackInfo));
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("CPO:SL", _skillLevel);
            info.AddValue("CPO:CDB", _criticalDamageBonus);
            info.AddValue("CPO:CC", _criticalChance);
            info.AddValue("CPO:S", _school);
            info.AddValue("CPO:II", _interruptionInfo);
            info.AddValue("CPO:C", _c);
        }

        private int _skillLevel;

        private float _criticalDamageBonus;

        private float _criticalChance;

        private PsiSchool _school;

        private InterruptibleAttackInfo _interruptionInfo;

        private double _c = 1.0;
    }
}
