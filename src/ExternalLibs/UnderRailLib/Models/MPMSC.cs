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
            _mv = info.GetInt32("MPMSC:MV");
            if (DataModelVersion.MinorVersion >= 483)
            {
                _sn = (SkillEnum) info.GetValue("MPMSC:SN", typeof(SkillEnum));
                return;
            }

            var text = info.GetString("MPMSC:SN");
            text = text?.Replace(" ", "");
            Enum.TryParse<SkillEnum>(text, true, out _sn);
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("MPMSC:MV", _mv);
            if (DataModelVersion.MinorVersion >= 483)
            {
                info.AddValue("MPMSC:SN", _sn);
            }
            else
            {
                info.AddValue("MPMSC:SN", _sn.ToString());
            }
        }

        private int _mv;
        private SkillEnum _sn;
    }
}
