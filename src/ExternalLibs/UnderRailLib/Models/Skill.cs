using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.TimelapseVertigo.Rules.Characters;

namespace UnderRailLib.Models
{
    [EncodedTypeName("S5")]
    [Serializable]
    public abstract class Skill : Stat<int>
    {
        protected Skill(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                _name = info.GetString("S5:N");
                _category = (SkillCategory)info.GetValue("S5:C", typeof(SkillCategory));
                return;
            }
            if (GetType() == typeof(Skill))
            {
                _name = info.GetString("_Name");
                _category = (SkillCategory)info.GetValue("_Category", typeof(SkillCategory));
                return;
            }
            _name = info.GetString("Skill+_Name");
            _category = (SkillCategory)info.GetValue("Skill+_Category", typeof(SkillCategory));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("S5:N", _name);
            info.AddValue("S5:C", _category);
        }

        private string _name;

        private SkillCategory _category;
    }
}
