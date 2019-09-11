using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.TimelapseVertigo.Playfield.CustomLogic.Data;

namespace UnderRailLib.Models
{
    [EncodedTypeName("PHESC")]
    [Serializable]
    public sealed class PHESC : Condition
    {
        private PHESC(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _e = (PlayerHouseElement) info.GetValue("PHESC:E", typeof(PlayerHouseElement));
            _v = info.GetString("PHESC:V");
            if (DataModelVersion.MinorVersion >= 275)
            {
                _i = info.GetBoolean("PHESC:I");
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("PHESC:E", _e);
            info.AddValue("PHESC:V", _v);
            info.AddValue("PHESC:I", _i);
        }

        private PlayerHouseElement _e;

        private string _v;

        private bool _i;
    }
}
