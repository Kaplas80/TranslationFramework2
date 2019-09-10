using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("ZTA")]
    [Serializable]
    public sealed class ZoneTransitionAction : BaseAction
    {
        private ZoneTransitionAction(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                _destination = (LocaleReference)info.GetValue("ZTA:D", typeof(LocaleReference));
                return;
            }
            if (GetType() == typeof(ZoneTransitionAction))
            {
                _destination = (LocaleReference)info.GetValue("_Destination", typeof(LocaleReference));
                return;
            }
            _destination = (LocaleReference)info.GetValue("ZoneTransitionAction+_Destination", typeof(LocaleReference));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("ZTA:D", _destination);
        }

        private LocaleReference _destination = new LocaleReference();
    }
}
