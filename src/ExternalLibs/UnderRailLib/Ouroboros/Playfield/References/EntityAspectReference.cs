using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models;
using UnderRailLib.Ouroboros.Common.Data;

namespace UnderRailLib.Ouroboros.Playfield.References
{
    [EncodedTypeName("EAR")]
    [Serializable]
    public sealed class EntityAspectReference<TAspect, TBaseAspect> : TypedObjectReference<TAspect>, ISerializable where TAspect : class, TBaseAspect where TBaseAspect : A
    {
        private EntityAspectReference(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MajorVersion != 0)
            {
                _playfieldName = info.GetString("EAR:PN");
                _zoneId = (Guid)info.GetValue("EAR:ZI", typeof(Guid));
                _areaId = (Guid)info.GetValue("EAR:AI", typeof(Guid));
                return;
            }
            if (GetType() == typeof(EntityAspectReference<TAspect, TBaseAspect>))
            {
                _playfieldName = info.GetString("_PlayfieldName");
                _zoneId = (Guid)info.GetValue("_ZoneId", typeof(Guid));
                _areaId = (Guid)info.GetValue("_AreaId", typeof(Guid));
                return;
            }
            _playfieldName = info.GetString("EntityAspectReference`2+_PlayfieldName");
            _zoneId = (Guid)info.GetValue("EntityAspectReference`2+_ZoneId", typeof(Guid));
            _areaId = (Guid)info.GetValue("EntityAspectReference`2+_AreaId", typeof(Guid));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("EAR:PN", _playfieldName);
            info.AddValue("EAR:ZI", _zoneId);
            info.AddValue("EAR:AI", _areaId);
        }

        private string _playfieldName;

        private Guid _zoneId;

        private Guid _areaId;
    }
}
