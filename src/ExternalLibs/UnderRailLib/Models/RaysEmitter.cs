using System;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Interfaces;

namespace UnderRailLib.Models
{
    [EncodedTypeName("RE")]
    [Serializable]
    public abstract class RaysEmitter : iID, ISerializable
    {
        protected RaysEmitter(SerializationInfo info, StreamingContext ctx)
        {
            if (DataModelVersion.MajorVersion != 0)
            {
                _id = (Guid)info.GetValue("RE:I", typeof(Guid));
                _carrier = (E4)info.GetValue("RE:C", typeof(E4));
                _location = (Vector3)info.GetValue("RE:L", typeof(Vector3));
                return;
            }
            if (GetType() == typeof(RaysEmitter))
            {
                _id = (Guid)info.GetValue("_Id", typeof(Guid));
                _carrier = (E4)info.GetValue("_Carrier", typeof(E4));
                _location = (Vector3)info.GetValue("_Location", typeof(Vector3));
                return;
            }
            _id = (Guid)info.GetValue("RaysEmitter+_Id", typeof(Guid));
            _carrier = (E4)info.GetValue("RaysEmitter+_Carrier", typeof(E4));
            _location = (Vector3)info.GetValue("RaysEmitter+_Location", typeof(Vector3));
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            info.AddValue("RE:I", _id);
            info.AddValue("RE:C", _carrier);
            info.AddValue("RE:L", _location);
        }

        private Guid _id;

        private E4 _carrier;

        private Vector3 _location;
    }
}
