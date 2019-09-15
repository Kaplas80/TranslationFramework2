using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Interfaces;

namespace UnderRailLib.Models
{
    [EncodedTypeName("O")]
    [Serializable]
    public class Observer : iID, iNM, ISerializable
    {
        protected Observer(SerializationInfo info, StreamingContext ctx)
        {
            if (DataModelVersion.MajorVersion != 0)
            {
                _id = (Guid)info.GetValue("O:I", typeof(Guid));
                _name = info.GetString("O:N");
                _observerCode = info.GetByte("O:OC");
                _observerMask = info.GetInt64("O:OM");
                _enabled = info.GetBoolean("O:E");
                return;
            }
            if (GetType() == typeof(Observer))
            {
                _id = (Guid)info.GetValue("_Id", typeof(Guid));
                _name = info.GetString("_Name");
                _observerCode = info.GetByte("_ObserverCode");
                _observerMask = info.GetInt64("_ObserverMask");
                _enabled = info.GetBoolean("_Enabled");
                return;
            }
            _id = (Guid)info.GetValue("Observer+_Id", typeof(Guid));
            _name = info.GetString("Observer+_Name");
            _observerCode = info.GetByte("Observer+_ObserverCode");
            _observerMask = info.GetInt64("Observer+_ObserverMask");
            _enabled = info.GetBoolean("Observer+_Enabled");
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            info.AddValue("O:I", _id);
            info.AddValue("O:N", _name);
            info.AddValue("O:OC", _observerCode);
            info.AddValue("O:OM", _observerMask);
            info.AddValue("O:E", _enabled);
        }

        private Guid _id;

        private string _name;

        public byte _observerCode;

        public long _observerMask;

        private bool _enabled;
    }
}
