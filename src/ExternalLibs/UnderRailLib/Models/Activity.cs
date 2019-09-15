using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Enums;
using UnderRailLib.Models.Interfaces;

namespace UnderRailLib.Models
{
    [EncodedTypeName("A2")]
    [Serializable]
    public abstract class Activity : iID, iNM, ISerializable
    {
        protected Activity(SerializationInfo info, StreamingContext ctx)
        {
            if (DataModelVersion.MajorVersion != 0)
            {
                _id = (Guid)info.GetValue("A2:I", typeof(Guid));
                _name = info.GetString("A2:N");
                _errorHandling = (eAEH)info.GetValue("A2:EH", typeof(eAEH));
                _parent = (Activity)info.GetValue("A2:P", typeof(Activity));
                _isAsync = info.GetBoolean("A2:IA");
                if (DataModelVersion.MajorVersion >= 11)
                {
                    _c = info.GetBoolean("A2:C");
                }
            }
            else
            {
                if (GetType() == typeof(Activity))
                {
                    _id = (Guid)info.GetValue("_Id", typeof(Guid));
                    _name = info.GetString("_Name");
                    _errorHandling = (eAEH)info.GetValue("_ErrorHandling", typeof(eAEH));
                    _parent = (Activity)info.GetValue("_Parent", typeof(Activity));
                    _isAsync = info.GetBoolean("_IsAsync");
                    return;
                }
                _id = (Guid)info.GetValue("Activity+_Id", typeof(Guid));
                _name = info.GetString("Activity+_Name");
                _errorHandling = (eAEH)info.GetValue("Activity+_ErrorHandling", typeof(eAEH));
                _parent = (Activity)info.GetValue("Activity+_Parent", typeof(Activity));
                _isAsync = info.GetBoolean("Activity+_IsAsync");
            }
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            info.AddValue("A2:I", _id);
            info.AddValue("A2:N", _name);
            info.AddValue("A2:EH", _errorHandling);
            info.AddValue("A2:P", _parent);
            info.AddValue("A2:IA", _isAsync);
            info.AddValue("A2:C", _c);
        }

        private Guid _id;

        private string _name;

        private eAEH _errorHandling;

        private Activity _parent;

        private bool _isAsync;

        private bool _c;
    }
}
