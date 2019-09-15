using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.TimelapseVertigo.Rules.Combat;

namespace UnderRailLib.Models
{
    [EncodedTypeName("DTF")]
    [Serializable]
    public struct DTF : ISerializable
    {
        private DTF(SerializationInfo info, StreamingContext ctx)
        {
            if (DataModelVersion.MinorVersion >= 3)
            {
                _value = info.GetSingle("V");
                _damageType = (DamageType)info.GetValue("T", typeof(DamageType));
            }
            else
            {
                _value = info.GetSingle("_Value");
                _damageType = (DamageType)info.GetValue("_DamageType", typeof(DamageType));
            }
            if (DataModelVersion.MinorVersion >= 89)
            {
                _a = info.GetDouble("A");
                _b = info.GetDouble("B");
                _c = info.GetInt32("C");
                return;
            }
            _a = 0.0;
            _b = 0.0;
            _c = 0;
        }
        
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("C", _c);
            info.AddValue("V", _value);
            info.AddValue("A", _a);
            info.AddValue("B", _b);
            info.AddValue("T", _damageType);
        }

        private DamageType _damageType;

        private int _c;

        private float _value;

        private double _a;

        private double _b;
    }
}
