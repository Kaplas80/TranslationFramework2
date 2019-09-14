using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.TimelapseVertigo.Rules.Characters;

namespace UnderRailLib.Models
{
    [EncodedTypeName("MPMSC")]
    [Serializable]
    public sealed class MPMSC : Condition
    {
        private MPMSC(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _minValue = info.GetInt32("MPMSC:MV");
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
            info.AddValue("MPMSC:MV", _minValue);
            if (DataModelVersion.MinorVersion >= 483)
            {
                info.AddValue("MPMSC:SN", _skillName);
            }
            else
            {
                var text = _skillName.Description();
                info.AddValue("MPMSC:SN", text);
            }
        }

        private int _minValue;
        private SkillEnum _skillName;
    }
}
