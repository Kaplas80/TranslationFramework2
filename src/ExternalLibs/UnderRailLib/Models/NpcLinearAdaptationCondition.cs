using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("NLAC")]
    [Serializable]
    public sealed class NpcLinearAdaptationCondition : Condition
    {
        private NpcLinearAdaptationCondition(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                _lowerLimit = info.GetSingle("NLAC:LL");
                _upperLimit = info.GetSingle("NLAC:UL");
                _npcName = info.GetString("NLAC:NN");
                return;
            }

            if (GetType() == typeof(NpcLinearAdaptationCondition))
            {
                _lowerLimit = info.GetSingle("_LowerLimit");
                _upperLimit = info.GetSingle("_UpperLimit");
                _npcName = info.GetString("_NpcName");
                return;
            }

            _lowerLimit = info.GetSingle("NpcLinearAdaptationCondition+_LowerLimit");
            _upperLimit = info.GetSingle("NpcLinearAdaptationCondition+_UpperLimit");
            _npcName = info.GetString("NpcLinearAdaptationCondition+_NpcName");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("NLAC:LL", _lowerLimit);
            info.AddValue("NLAC:UL", _upperLimit);
            info.AddValue("NLAC:NN", _npcName);
        }

        private float _lowerLimit;

        private float _upperLimit;

        private string _npcName;
    }
}
