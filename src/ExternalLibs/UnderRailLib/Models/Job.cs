using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("J1")]
    [Serializable]
    public abstract class Job : ISerializable
    {
        protected Job(SerializationInfo info, StreamingContext ctx)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                _name = info.GetString("J1:N");
                return;
            }

            if (GetType() == typeof(Job))
            {
                _name = info.GetString("_Name");
                return;
            }

            _name = info.GetString("Job+_Name");
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            info.AddValue("J1:N", _name);
        }

        private string _name;
    }
}
