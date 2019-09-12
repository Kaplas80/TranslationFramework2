using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.TimelapseVertigo.Rules.Combat;

namespace UnderRailLib.Models
{
    [EncodedTypeName("DR2")]
    [Serializable]
    public struct DR2 : ISerializable
    {
        private DR2(SerializationInfo info, StreamingContext ctx)
        {
            if (DataModelVersion.MinorVersion < 3)
            {
                _value = info.GetInt32("_Value");
                _damageType = (DamageType)info.GetValue("_DamageType", typeof(DamageType));
                _r = 0.0;
                return;
            }
            _value = info.GetInt32("V");
            _damageType = (DamageType)info.GetValue("T", typeof(DamageType));
            if (DataModelVersion.MinorVersion >= 88)
            {
                _r = info.GetDouble("R");
                return;
            }
            _r = 0.0;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("V", _value);
            info.AddValue("T", _damageType);
            info.AddValue("R", _r);
        }

        private DamageType _damageType;

        private int _value;

        private double _r;
    }
}
