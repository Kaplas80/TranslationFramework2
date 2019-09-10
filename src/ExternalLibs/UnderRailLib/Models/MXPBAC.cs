using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.TimelapseVertigo.Rules.Characters;

namespace UnderRailLib.Models
{
    [EncodedTypeName("MXPBAC")]
    [Serializable]
    public sealed class MXPBAC : Condition
    {
        private MXPBAC(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _mv = info.GetInt32("MXPBAC:MV");
            if (DataModelVersion.MinorVersion >= 411)
            {
                _ban = (BaseAbilityEnum)info.GetValue("MXPBAC:BAN", typeof(BaseAbilityEnum));
                return;
            }
            var str = info.GetString("MXPBAC:BAN");
            Enum.TryParse(str, true, out _ban);
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("MXPBAC:MV", _mv);
            if (DataModelVersion.MinorVersion >= 411)
            {
                info.AddValue("MXPBAC:BAN", _ban);
            }
            else
            {
                info.AddValue("MXPBAC:BAN", _ban.ToString());
            }
        }

        private int _mv;
        private BaseAbilityEnum _ban;
    }

}
