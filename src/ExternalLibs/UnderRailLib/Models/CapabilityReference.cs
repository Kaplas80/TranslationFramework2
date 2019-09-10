using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("CR")]
    [Serializable]
    public sealed class CapabilityReference : ISerializable
    {
        private CapabilityReference(SerializationInfo info, StreamingContext ctx)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                _cababilityFullTypeName = info.GetString("CR:CFTN");
                return;
            }

            if (GetType() == typeof(CapabilityReference))
            {
                _cababilityFullTypeName = info.GetString("_CapabilityFullTypeName");
                return;
            }

            _cababilityFullTypeName = info.GetString("CapabilityReference+_CapabilityFullTypeName");
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            info.AddValue("CR:CFTN", _cababilityFullTypeName);
        }

        private string _cababilityFullTypeName;
    }
}
