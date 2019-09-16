using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Interfaces;
using UnderRailLib.Ouroboros.Common.Data;

namespace UnderRailLib.Ouroboros.Playfield.References
{
    [EncodedTypeName("AR1")]
    [Serializable]
    public sealed class ActionReference<T> : TypedObjectReference<T> where T : class, iACT
    {
        private ActionReference(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MajorVersion != 0)
            {
                _zoneId = (Guid)info.GetValue("AR1:ZI", typeof(Guid?));
                _playfieldName = info.GetString("AR1:PN");
                return;
            }
            if (GetType() == typeof(TypedObjectReference<T>))
            {
                _zoneId = (Guid)info.GetValue("_ZoneId", typeof(Guid));
                _playfieldName = info.GetString("_PlayfieldName");
                return;
            }
            _zoneId = (Guid)info.GetValue("ActionReference`1+_ZoneId", typeof(Guid));
            _playfieldName = info.GetString("ActionReference`1+_PlayfieldName");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("AR1:ZI", _zoneId);
            info.AddValue("AR1:PN", _playfieldName);
        }
     
        private string _playfieldName;

		private Guid _zoneId;
    }
}
