using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("ZSK")]
    [Serializable]
    public struct ZSK : ISerializable
    {
        private ZSK(SerializationInfo info, StreamingContext ctx)
        {
            if (DataModelVersion.MajorVersion >= 6)
            {
                _intanceId = info.GetString("I");
                _areaId = (Guid)info.GetValue("A", typeof(Guid));
                return;
            }
            _intanceId = info.GetString("_InstanceId");
            _areaId = (Guid)info.GetValue("_AreaId", typeof(Guid));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("I", _intanceId);
            info.AddValue("A", _areaId);
        }

        private Guid _areaId;

        private string _intanceId;
    }
}
