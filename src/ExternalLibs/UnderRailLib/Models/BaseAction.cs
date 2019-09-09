using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("BA")]
    [Serializable]
    public abstract class BaseAction : ISerializable
    {
        protected BaseAction(SerializationInfo info, StreamingContext ctx)
        {
            if (DataModelVersion.MajorVersion != 0)
            {
                _name = info.GetString("BA:N");
                return;
            }
            
            if (GetType() == typeof(BaseAction))
            {
                _name = info.GetString("_Name");
                return;
            }
            _name = info.GetString("BaseAction+_Name");
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            info.AddValue("BA:N", _name);
        }

        private string _name;
    }
}