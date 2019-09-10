using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("RIFPA")]
    [Serializable]
    public sealed class RemoveItemFromPlayerAction : BaseAction
    {
        private RemoveItemFromPlayerAction(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                _itemCode = info.GetString("RIFPA:IC");
                _stacks = info.GetInt32("RIFPA:S");
                _searchGear = info.GetBoolean("RIFPA:SG");
                return;
            }

            if (GetType() == typeof(RemoveItemFromPlayerAction))
            {
                _itemCode = info.GetString("_ItemCode");
                _stacks = info.GetInt32("_Stacks");
                _searchGear = info.GetBoolean("_SearchGear");
                return;
            }

            _itemCode = info.GetString("RemoveItemFromPlayerAction+_ItemCode");
            _stacks = info.GetInt32("RemoveItemFromPlayerAction+_Stacks");
            _searchGear = info.GetBoolean("RemoveItemFromPlayerAction+_SearchGear");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("RIFPA:IC", _itemCode);
            info.AddValue("RIFPA:S", _stacks);
            info.AddValue("RIFPA:SG", _searchGear);
        }

        private string _itemCode;

        private int _stacks = 1;

        private bool _searchGear = true;
    }
}
