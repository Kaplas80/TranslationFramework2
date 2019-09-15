using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.TimelapseVertigo.Rules.Combat;

namespace UnderRailLib.Models
{
    [EncodedTypeName("D3")]
    [Serializable]
    public struct Damage : ISerializable
    {
        private Damage(SerializationInfo info, StreamingContext ctx)
        {
            if (DataModelVersion.MinorVersion >= 3)
            {
                _amount = info.GetInt32("A");
                _damageType = (DamageType)info.GetValue("T", typeof(DamageType));
                return;
            }
            _amount = info.GetInt32("_Amount");
            _damageType = (DamageType)info.GetValue("_DamageType", typeof(DamageType));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("A", _amount);
            info.AddValue("T", _damageType);
        }

        private DamageType _damageType;

        private int _amount;
    }
}
