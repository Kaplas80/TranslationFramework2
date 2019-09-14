using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.TimelapseVertigo.Rules.Characters;

namespace UnderRailLib.Models
{
    [EncodedTypeName("MPUSC")]
    [Serializable]
    public sealed class MPUSC : Condition
    {
        private MPUSC(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MinorVersion == 0)
            {
                _minValue = info.GetInt32("_MinValue");
                var text = info.GetString("_SkillName");
                text = text?.Replace(" ", "");

                Enum.TryParse(text, true, out _skillName);
                return;
            }

            _minValue = info.GetInt32("MPUSC:MV");
            if (DataModelVersion.MinorVersion >= 483)
            {
                _skillName = (SkillEnum) info.GetValue("MPUSC:SN", typeof(SkillEnum));
                return;
            }

            var text2 = info.GetString("MPUSC:SN");
            text2 = text2?.Replace(" ", "");

            Enum.TryParse(text2, true, out _skillName);
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("MPUSC:MV", _minValue);
            if (DataModelVersion.MinorVersion >= 483)
            {
                info.AddValue("MPUSC:SN", _skillName);
            }
            else
            {
                var text = _skillName.Description();
                info.AddValue("MPUSC:SN", text);
            }
        }

        private int _minValue;

        private SkillEnum _skillName;
    }
}
