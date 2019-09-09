using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("C")]
    [Serializable]
    public abstract class Condition : ISerializable
    {
        protected Condition(SerializationInfo info, StreamingContext ctx)
        {
            if (DataModelVersion.MajorVersion != 0)
            {
                _name = info.GetString("C:N");
                return;
            }
            if (GetType() == typeof(Condition))
            {
                _name = info.GetString("_Name");
                return;
            }
            _name = info.GetString("Condition+_Name");
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            info.AddValue("C:N", _name);
        }

        private string _name;
    }
}
