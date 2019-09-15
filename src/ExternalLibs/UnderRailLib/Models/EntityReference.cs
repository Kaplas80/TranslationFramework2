using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Ouroboros.Common.Data;

namespace UnderRailLib.Models
{
    [EncodedTypeName("ER")]
    [Serializable]
    public sealed class EntityReference : TypedObjectReference<E4>
    {
        private EntityReference(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MajorVersion != 0)
            {
                _playfieldName = info.GetString("ER:PN");
                _zoneId = (Guid) info.GetValue("ER:ZI", typeof(Guid));
                _areaId = (Guid) info.GetValue("ER:AI", typeof(Guid));
                return;
            }

            if (GetType() == typeof(EntityReference))
            {
                _playfieldName = info.GetString("_PlayfieldName");
                _zoneId = (Guid) info.GetValue("_ZoneId", typeof(Guid));
                _areaId = (Guid) info.GetValue("_AreaId", typeof(Guid));
                return;
            }

            _playfieldName = info.GetString("EntityReference+_PlayfieldName");
            _zoneId = (Guid) info.GetValue("EntityReference+_ZoneId", typeof(Guid));
            _areaId = (Guid) info.GetValue("EntityReference+_AreaId", typeof(Guid));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("ER:PN", _playfieldName);
            info.AddValue("ER:ZI", _zoneId);
            info.AddValue("ER:AI", _areaId);
        }

        private string _playfieldName;

        private Guid _zoneId;

        private Guid _areaId;
    }
}
