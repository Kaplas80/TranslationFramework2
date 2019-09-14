using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Interfaces;

namespace UnderRailLib.Models
{
    [EncodedTypeName("MSFE1")]
    [Serializable]
    public sealed class ModifySkillFlatEffect : ModifyStatFlatEffect, iMS, iSKFX
    {
        private ModifySkillFlatEffect(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                _skillName = info.GetString("MSFE1:SN");
                return;
            }
            if (GetType() == typeof(ModifySkillFlatEffect))
            {
                _skillName = info.GetString("_SkillName");
                return;
            }
            _skillName = info.GetString("ModifySkillFlatEffect+_SkillName");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("MSFE1:SN", _skillName);
        }

        private string _skillName;
    }
}
