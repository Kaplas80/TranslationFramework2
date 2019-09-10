using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("PHIC")]
    [Serializable]
    public sealed class PlayerHasItemCondition : Condition
    {
        private PlayerHasItemCondition(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                _itemCode = info.GetString("PHIC:IC");
                _stacks = info.GetInt32("PHIC:S");
                _inverse = info.GetBoolean("PHIC:I");
                _searchGear = info.GetBoolean("PHIC:SG");
                if (DataModelVersion.MinorVersion >= 120)
                {
                    _n = (info.GetValue("PHIC:N", typeof(Guid?)) as Guid?);
                }
            }
            else
            {
                if (GetType() == typeof(PlayerHasItemCondition))
                {
                    _itemCode = info.GetString("_ItemCode");
                    _stacks = info.GetInt32("_Stacks");
                    _inverse = info.GetBoolean("_Inverse");
                    _searchGear = info.GetBoolean("_SearchGear");
                    return;
                }

                _itemCode = info.GetString("PlayerHasItemCondition+_ItemCode");
                _stacks = info.GetInt32("PlayerHasItemCondition+_Stacks");
                _inverse = info.GetBoolean("PlayerHasItemCondition+_Inverse");
                _searchGear = info.GetBoolean("PlayerHasItemCondition+_SearchGear");
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("PHIC:IC", _itemCode);
            info.AddValue("PHIC:S", _stacks);
            info.AddValue("PHIC:I", _inverse);
            info.AddValue("PHIC:SG", _searchGear);
            if (DataModelVersion.MinorVersion >= 120)
            {
                info.AddValue("PHIC:N", _n);
            }
        }

        private string _itemCode;

        private int _stacks = 1;

        private bool _inverse;

        private bool _searchGear = true;

        private Guid? _n;
    }
}
