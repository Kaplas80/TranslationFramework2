using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("PIIAC")]
    [Serializable]
    public sealed class PlayerIsInAreaCondition : Condition
    {
        private PlayerIsInAreaCondition(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                _areaName = info.GetString("PIIAC:AN");
                _inverse = info.GetBoolean("PIIAC:I");
                return;
            }

            if (GetType() == typeof(PlayerIsInAreaCondition))
            {
                _areaName = info.GetString("_AreaName");
                _inverse = info.GetBoolean("_Inverse");
                return;
            }

            _areaName = info.GetString("PlayerIsInAreaCondition+_AreaName");
            _inverse = info.GetBoolean("PlayerIsInAreaCondition+_Inverse");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("PIIAC:AN", _areaName);
            info.AddValue("PIIAC:I", _inverse);
        }

        private string _areaName;
        private bool _inverse;
    }
}
