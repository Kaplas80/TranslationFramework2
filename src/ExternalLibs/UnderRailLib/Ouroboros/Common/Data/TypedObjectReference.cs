using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Interfaces;

namespace UnderRailLib.Ouroboros.Common.Data
{
    [EncodedTypeName("TOR")]
    [Serializable]
    public abstract class TypedObjectReference<T> : iREF, ISerializable where T : class
    {
        protected TypedObjectReference(SerializationInfo info, StreamingContext ctx)
        {
            if (DataModelVersion.MajorVersion != 0)
            {
                if (GetType().Name == "FeatReference" && DataModelVersion.MinorVersion < 17)
                {
                    return;
                }

                _objectId = (Guid?) info.GetValue("TOR:OI", typeof(Guid?));
                _name = info.GetString("TOR:N");
            }
            else
            {
                if (GetType() == typeof(TypedObjectReference<T>))
                {
                    _objectId = (Guid?) info.GetValue("_ObjectId", typeof(Guid?));
                    _name = info.GetString("_Name");
                    return;
                }

                _objectId = (Guid?) info.GetValue("TypedObjectReference`1+_ObjectId", typeof(Guid?));
                _name = info.GetString("TypedObjectReference`1+_Name");
            }
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (GetType().Name == "FeatReference" && DataModelVersion.MinorVersion < 17)
            {
                return;
            }

            info.AddValue("TOR:OI", _objectId);
            info.AddValue("TOR:N", _name);
        }

        private Guid? _objectId;

        private string _name;
    }
}
