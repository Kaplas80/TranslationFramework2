using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Interfaces;

namespace UnderRailLib.Models
{
    [EncodedTypeName("MSPE1")]
    [Serializable]
    public sealed class ModifySkillPercentageEffect : ModifyStatPercentageEffect, iSKFX
    {
        private ModifySkillPercentageEffect(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                _skillName = info.GetString("MSPE1:SN");
                _applyToModifiedValue = info.GetBoolean("MSPE1:ATMV");
                return;
            }
            if (GetType() == typeof(ModifySkillPercentageEffect))
            {
                _skillName = info.GetString("_SkillName");
                _applyToModifiedValue = info.GetBoolean("_ApplyToModifiedValue");
                return;
            }
            _skillName = info.GetString("ModifySkillPercentageEffect+_SkillName");
            _applyToModifiedValue = info.GetBoolean("ModifySkillPercentageEffect+_ApplyToModifiedValue");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("MSPE1:SN", _skillName);
            info.AddValue("MSPE1:ATMV", _applyToModifiedValue);
        }

        private string _skillName;

        private bool _applyToModifiedValue;
    }
}
