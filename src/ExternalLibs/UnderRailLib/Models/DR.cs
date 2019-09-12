using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.TimelapseVertigo.Rules.Combat;

namespace UnderRailLib.Models
{
    [EncodedTypeName("DR")]
    [Serializable]
    public struct DR : ISerializable
    {
        private DR(SerializationInfo info, StreamingContext ctx)
        {
            if (DataModelVersion.MinorVersion >= 3)
            {
                _lowerBoundary = info.GetInt32("L");
                _upperBoundary = info.GetInt32("U");
                _damageType = (DamageType) info.GetValue("T", typeof(DamageType));
                return;
            }

            _lowerBoundary = info.GetInt32("_LowerBoundary");
            _upperBoundary = info.GetInt32("_UpperBoundary");
            _damageType = (DamageType) info.GetValue("_DamageType", typeof(DamageType));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("L", _lowerBoundary);
            info.AddValue("U", _upperBoundary);
            info.AddValue("T", _damageType);
        }

        private DamageType _damageType;

        private int _lowerBoundary;

        private int _upperBoundary;
    }
}
