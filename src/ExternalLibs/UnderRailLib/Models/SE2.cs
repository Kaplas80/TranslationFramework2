using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Interfaces;

namespace UnderRailLib.Models
{
    [EncodedTypeName("SE2")]
    [Serializable]
    public abstract class SE2 : ISerializable
    {
        protected SE2(SerializationInfo info, StreamingContext ctx)
        {
            _priority = info.GetInt32("SE2:P");
            if (DataModelVersion.MinorVersion >= 156)
            {
                _condition = (info.GetValue("SE2:C", typeof(iCOND)) as iCOND);
            }
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            info.AddValue("SE2:P", _priority);
            if (DataModelVersion.MinorVersion >= 156)
            {
                info.AddValue("SE2:C", _condition);
            }
        }

        private int _priority;

        private iCOND _condition;
    }
}