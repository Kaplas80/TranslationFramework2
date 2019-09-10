using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Enums;
using UnderRailLib.Models.Interfaces;

namespace UnderRailLib.Models
{
    [EncodedTypeName("IO")]
    [Serializable]
    public sealed class ItemOpportunity : ISerializable
    {
        private ItemOpportunity(SerializationInfo info, StreamingContext ctx)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                _generator = (anu) info.GetValue("IO:G", typeof(anu));
                _chance = info.GetSingle("IO:C");
                _subGroup = info.GetInt32("IO:SG");
                _autoEquipMode = (AutoEquip) info.GetValue("IO:AEM", typeof(AutoEquip));
                _lootable = info.GetBoolean("IO:L");
                _amount = info.GetInt32("IO:A");
                if (DataModelVersion.MinorVersion >= 7)
                {
                    _stealable = info.GetBoolean("IO:S");
                }
            }
            else
            {
                if (GetType() == typeof(ItemOpportunity))
                {
                    _generator = (anu) info.GetValue("_Generator", typeof(anu));
                    _chance = info.GetSingle("_Chance");
                    _subGroup = info.GetInt32("_SubGroup");
                    _autoEquipMode = (AutoEquip) info.GetValue("_AutoEquipMode", typeof(AutoEquip));
                    _lootable = info.GetBoolean("_Lootable");
                    _amount = info.GetInt32("_Amount");
                    return;
                }

                _generator = (anu) info.GetValue("ItemOpportunity+_Generator", typeof(anu));
                _chance = info.GetSingle("ItemOpportunity+_Chance");
                _subGroup = info.GetInt32("ItemOpportunity+_SubGroup");
                _autoEquipMode = (AutoEquip) info.GetValue("ItemOpportunity+_AutoEquipMode", typeof(AutoEquip));
                _lootable = info.GetBoolean("ItemOpportunity+_Lootable");
                _amount = info.GetInt32("ItemOpportunity+_Amount");
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            info.AddValue("IO:G", _generator);
            info.AddValue("IO:C", _chance);
            info.AddValue("IO:SG", _subGroup);
            info.AddValue("IO:AEM", _autoEquipMode);
            info.AddValue("IO:L", _lootable);
            if (DataModelVersion.MinorVersion >= 7)
            {
                info.AddValue("IO:S", _stealable);
            }
            info.AddValue("IO:A", _amount);
        }

        private anu _generator;

        private float _chance = 1f;

        private int _subGroup;

        private AutoEquip _autoEquipMode;

        private bool _lootable = true;

        private bool _stealable = true;

        private int _amount = 1;
    }
}
