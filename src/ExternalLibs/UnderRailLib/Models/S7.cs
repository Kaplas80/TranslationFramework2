using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.TimelapseVertigo.Rules.Combat;

namespace UnderRailLib.Models
{
    [EncodedTypeName("S7")]
    [Serializable]
    public struct S7 : ISerializable
    {
        private S7(SerializationInfo A_0, StreamingContext A_1)
        {
            if (DataModelVersion.MinorVersion >= 3)
            {
                _value = A_0.GetInt32("V");
                _impactSpeed = (ImpactSpeed)A_0.GetValue("I", typeof(ImpactSpeed));
                return;
            }
            _value = A_0.GetInt32("_Value");
            _impactSpeed = (ImpactSpeed)A_0.GetValue("_ImpactSpeed", typeof(ImpactSpeed));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("V", _value);
            info.AddValue("I", _impactSpeed);
        }

        private ImpactSpeed _impactSpeed;

        private int _value;
    }
}
