using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.TimelapseVertigo.Rules.Characters;

namespace UnderRailLib.Models
{
    [EncodedTypeName("MAXPUSC")]
    [Serializable]
    public sealed class MAXPUSC : Condition
    {
        private MAXPUSC(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _mv = info.GetInt32("MAXPUSC:MV");
            if (DataModelVersion.MinorVersion >= 483)
            {
                _skillName = (SkillEnum) info.GetValue("MAXPUSC:SN", typeof(SkillEnum));
                return;
            }

            var text = info.GetString("MAXPUSC:SN");
            text = text?.Replace(" ", "");
            Enum.TryParse(text, true, out _skillName);
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("MAXPUSC:MV", _mv);
            if (DataModelVersion.MinorVersion >= 483)
            {
                info.AddValue("MAXPUSC:SN", _skillName);
            }
            else
            {
                var text = _skillName.Description();
                info.AddValue("MAXPUSC:SN", text);
            }
        }

        private int _mv;
        private SkillEnum _skillName;
    }
}
