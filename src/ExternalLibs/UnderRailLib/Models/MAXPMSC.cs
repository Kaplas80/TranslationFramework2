using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.TimelapseVertigo.Rules.Characters;

namespace UnderRailLib.Models
{
    [EncodedTypeName("MAXPMSC")]
    [Serializable]
    public sealed class MAXPMSC : Condition
    {
        private MAXPMSC(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _mv = info.GetInt32("MPMSC:MV");
            if (DataModelVersion.MinorVersion >= 483)
            {
                _skillName = (SkillEnum) info.GetValue("MPMSC:SN", typeof(SkillEnum));
                return;
            }

            var text = info.GetString("MPMSC:SN");
            text = text?.Replace(" ", "");
            Enum.TryParse(text, true, out _skillName);
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("MPMSC:MV", _mv);
            if (DataModelVersion.MinorVersion >= 483)
            {
                info.AddValue("MPMSC:SN", _skillName);
            }
            else
            {
                var text = _skillName.ToString();
                info.AddValue("MPMSC:SN", text);
            }
        }

        private int _mv;
        private SkillEnum _skillName;
    }
}
