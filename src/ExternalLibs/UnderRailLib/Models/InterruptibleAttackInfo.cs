using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("IAI")]
    [Serializable]
    public sealed class InterruptibleAttackInfo : ISerializable
    {
        private InterruptibleAttackInfo(SerializationInfo info, StreamingContext ctx)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                _pushbackReduction = info.GetDouble("IAI:PR");
                _interruptionResistChance = info.GetDouble("IAI:IRC");
                return;
            }
            if (GetType() == typeof(InterruptibleAttackInfo))
            {
                _pushbackReduction = info.GetDouble("PushbackReduction");
                _interruptionResistChance = info.GetDouble("InterruptionResistChance");
                return;
            }
            _pushbackReduction = info.GetDouble("InterruptibleAttackInfo+PushbackReduction");
            _interruptionResistChance = info.GetDouble("InterruptibleAttackInfo+InterruptionResistChance");
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            info.AddValue("IAI:PR", _pushbackReduction);
            info.AddValue("IAI:IRC", _interruptionResistChance);
        }

        public double _pushbackReduction;

        public double _interruptionResistChance;
    }
}
