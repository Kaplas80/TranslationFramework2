using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Interfaces;

namespace UnderRailLib.Models
{
    [EncodedTypeName("ZTT")]
    [Serializable]
    public sealed class ZoneTransitionTrigger : Trigger
    {
        private ZoneTransitionTrigger(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                _destination = (LocaleReference)info.GetValue("ZTT:D", typeof(LocaleReference));
            }
            else if (GetType() == typeof(ZoneTransitionTrigger))
            {
                _destination = (LocaleReference)info.GetValue("_Destination", typeof(LocaleReference));
            }
            else
            {
                _destination = (LocaleReference)info.GetValue("ZoneTransitionTrigger+_Destination", typeof(LocaleReference));
            }
            if (DataModelVersion.MinorVersion >= 334)
            {
                _condition = (info.GetValue("ZTT:C", typeof(iCOND)) as iCOND);
            }
            if (DataModelVersion.MinorVersion >= 339)
            {
                _o = info.GetBoolean("ZTT:O");
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("ZTT:D", _destination);
            if (DataModelVersion.MinorVersion >= 334)
            {
                info.AddValue("ZTT:C", _condition);
            }

            if (DataModelVersion.MinorVersion >= 339)
            {
                info.AddValue("ZTT:O", _o);
            }
        }

        private LocaleReference _destination = new LocaleReference();

        private iCOND _condition;

        private bool _o;
    }
}
