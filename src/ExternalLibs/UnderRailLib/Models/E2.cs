using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Interfaces;

namespace UnderRailLib.Models
{
    [EncodedTypeName("E2")]
    [Serializable]
    public abstract class E2 : iID, iNM, ISerializable
    {
        protected E2(SerializationInfo info, StreamingContext ctx)
        {
            if (DataModelVersion.MajorVersion != 0)
            {
                _id = (Guid)info.GetValue("E2:I", typeof(Guid));
                _name = info.GetString("E2:N");
                return;
            }
            _id = (Guid)info.GetValue("_Id", typeof(Guid));
            _name = info.GetString("_Name");
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            info.AddValue("E2:I", _id);
            info.AddValue("E2:N", _name);
        }

        protected Guid _id;

        protected string _name;
    }
}
