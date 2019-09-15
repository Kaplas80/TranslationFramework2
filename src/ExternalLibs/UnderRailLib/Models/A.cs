using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Interfaces;

namespace UnderRailLib.Models
{
    [EncodedTypeName("A")]
    [Serializable]
    public abstract class A : ISerializable
    {
        protected A(SerializationInfo info, StreamingContext ctx)
        {
            if (DataModelVersion.MajorVersion != 0)
            {
                _container = (iIAC)info.GetValue("A:C", typeof(iIAC));
                return;
            }
            _container = (info.GetValue("_Container", typeof(iIAC)) as iIAC);
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            info.AddValue("A:C", _container);
        }

        private iIAC _container;
    }
}
