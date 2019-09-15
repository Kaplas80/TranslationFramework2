using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Enums;
using UnderRailLib.Models.Interfaces;

namespace UnderRailLib.Models
{
    [EncodedTypeName("LS")]
    [Serializable]
    public sealed class LightStamp : ISerializable
    {
        private LightStamp(SerializationInfo info, StreamingContext ctx)
        {
            if (DataModelVersion.MajorVersion != 0)
            {
                _sourceType = (eLST)info.GetValue("LS:S", typeof(eLST));
                _correlation = (Guid)info.GetValue("LS:C", typeof(Guid));
                _lightFragment = (iLFR)info.GetValue("LS:L", typeof(iLFR));
                return;
            }
            if (GetType() == typeof(LightStamp))
            {
                _sourceType = (eLST)info.GetValue("<SourceType>k__BackingField", typeof(eLST));
                _correlation = (Guid)info.GetValue("<CorrelationId>k__BackingField", typeof(Guid));
                _lightFragment = (iLFR)info.GetValue("<LightFragment>k__BackingField", typeof(iLFR));
                return;
            }
            _sourceType = (eLST)info.GetValue("LightStamp+<SourceType>k__BackingField", typeof(eLST));
            _correlation = (Guid)info.GetValue("LightStamp+<CorrelationId>k__BackingField", typeof(Guid));
            _lightFragment = (iLFR)info.GetValue("LightStamp+<LightFragment>k__BackingField", typeof(iLFR));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            info.AddValue("LS:S", _sourceType);
            info.AddValue("LS:C", _correlation);
            info.AddValue("LS:L", _lightFragment);
        }

        private eLST _sourceType;

        private Guid _correlation;

        private iLFR _lightFragment;
    }
}
