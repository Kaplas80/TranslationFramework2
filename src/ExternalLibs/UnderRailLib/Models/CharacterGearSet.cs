using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.EventArgs;
using UnderRailLib.TimelapseVertigo.Rules.Characters;
using UnderRailLib.TimelapseVertigo.Rules.Items;

namespace UnderRailLib.Models
{
    [EncodedTypeName("CGS")]
    [Serializable]
    public sealed class CharacterGearSet : ISerializable
    {
        private CharacterGearSet(SerializationInfo info, StreamingContext ctx)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                _helmet = (ItemSlot<HeadwearItemInstance>) info.GetValue("CGS:H", typeof(ItemSlot<HeadwearItemInstance>));
                _armor = (ItemSlot<ArmorSuitItemInstance>) info.GetValue("CGS:A", typeof(ItemSlot<ArmorSuitItemInstance>));
                _belt = (ItemSlot<BeltItemInstance>) info.GetValue("CGS:B", typeof(ItemSlot<BeltItemInstance>));
                _boots = (ItemSlot<FootwearItemInstance>) info.GetValue("CGS:B1", typeof(ItemSlot<FootwearItemInstance>));
                _shieldEmitter = (ItemSlot<ShieldEmitterItemInstance>) info.GetValue("CGS:SE", typeof(ItemSlot<ShieldEmitterItemInstance>));
                SerializationHelper.ReadArray("CGS:QSI", ref _quickSlotItems, info);
                SerializationHelper.ReadDictionary("CGS:W", ref _weapons, info);
                _availableQuickSlots = info.GetInt32("CGS:AQS");
                if (DataModelVersion.MajorVersion >= 5)
                {
                    SerializationHelper.ReadEvent("CGS:IE", ref _itemEquipped, info);
                    SerializationHelper.ReadEvent("CGS:IU", ref _itemUnequipped, info);
                    return;
                }

                _itemEquipped = (EventHandler<ItemEventArgs>) info.GetValue("CGS:IE", typeof(EventHandler<ItemEventArgs>));
                _itemUnequipped = (EventHandler<ItemEventArgs>) info.GetValue("CGS:IU", typeof(EventHandler<ItemEventArgs>));
            }
            else
            {
                if (GetType() == typeof(CharacterGearSet))
                {
                    _helmet = (ItemSlot<HeadwearItemInstance>) info.GetValue("_Helmet", typeof(ItemSlot<HeadwearItemInstance>));
                    _armor = (ItemSlot<ArmorSuitItemInstance>) info.GetValue("_Armor", typeof(ItemSlot<ArmorSuitItemInstance>));
                    _belt = (ItemSlot<BeltItemInstance>) info.GetValue("_Belt", typeof(ItemSlot<BeltItemInstance>));
                    _boots = (ItemSlot<FootwearItemInstance>) info.GetValue("_Boots", typeof(ItemSlot<FootwearItemInstance>));
                    _shieldEmitter = (ItemSlot<ShieldEmitterItemInstance>) info.GetValue("_ShieldEmitter", typeof(ItemSlot<ShieldEmitterItemInstance>));
                    _quickSlotItems = (ItemSlot<CombatUtilityItemInstance>[]) info.GetValue("_QuickSlotItems", typeof(ItemSlot<CombatUtilityItemInstance>[]));
                    _weapons = (Dictionary<WeaponKey, ItemSlot<WeaponItemInstance>>) info.GetValue("_Weapons",
                        typeof(Dictionary<WeaponKey, ItemSlot<WeaponItemInstance>>));
                    _availableQuickSlots = info.GetInt32("_AvailableQuickSlots");
                    _itemEquipped = (EventHandler<ItemEventArgs>) info.GetValue("ItemEquipped", typeof(EventHandler<ItemEventArgs>));
                    _itemUnequipped = (EventHandler<ItemEventArgs>) info.GetValue("ItemUnequipped", typeof(EventHandler<ItemEventArgs>));
                    return;
                }

                _helmet = (ItemSlot<HeadwearItemInstance>) info.GetValue("CharacterGearSet+_Helmet", typeof(ItemSlot<HeadwearItemInstance>));
                _armor = (ItemSlot<ArmorSuitItemInstance>) info.GetValue("CharacterGearSet+_Armor", typeof(ItemSlot<ArmorSuitItemInstance>));
                _belt = (ItemSlot<BeltItemInstance>) info.GetValue("CharacterGearSet+_Belt", typeof(ItemSlot<BeltItemInstance>));
                _boots = (ItemSlot<FootwearItemInstance>) info.GetValue("CharacterGearSet+_Boots", typeof(ItemSlot<FootwearItemInstance>));
                _shieldEmitter = (ItemSlot<ShieldEmitterItemInstance>) info.GetValue("CharacterGearSet+_ShieldEmitter", typeof(ItemSlot<ShieldEmitterItemInstance>));
                _quickSlotItems = (ItemSlot<CombatUtilityItemInstance>[]) info.GetValue("CharacterGearSet+_QuickSlotItems", typeof(ItemSlot<CombatUtilityItemInstance>[]));
                _weapons = (Dictionary<WeaponKey, ItemSlot<WeaponItemInstance>>) info.GetValue("CharacterGearSet+_Weapons",
                    typeof(Dictionary<WeaponKey, ItemSlot<WeaponItemInstance>>));
                _availableQuickSlots = info.GetInt32("CharacterGearSet+_AvailableQuickSlots");
                _itemEquipped = (EventHandler<ItemEventArgs>) info.GetValue("CharacterGearSet+ItemEquipped", typeof(EventHandler<ItemEventArgs>));
                _itemUnequipped = (EventHandler<ItemEventArgs>) info.GetValue("CharacterGearSet+ItemUnequipped", typeof(EventHandler<ItemEventArgs>));
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            info.AddValue("CGS:H", _helmet);
            info.AddValue("CGS:A", _armor);
            info.AddValue("CGS:B", _belt);
            info.AddValue("CGS:B1", _boots);
            info.AddValue("CGS:SE", _shieldEmitter);
            SerializationHelper.WriteArray("CGS:QSI", _quickSlotItems, info);
            SerializationHelper.WriteDictionary("CGS:W", _weapons, info);
            info.AddValue("CGS:AQS", _availableQuickSlots);
            SerializationHelper.WriteEvent("CGS:IE", _itemEquipped, info);
            SerializationHelper.WriteEvent("CGS:IU", _itemUnequipped, info);
        }

        private ItemSlot<HeadwearItemInstance> _helmet;

        private ItemSlot<ArmorSuitItemInstance> _armor;

        private ItemSlot<BeltItemInstance> _belt;

        private ItemSlot<FootwearItemInstance> _boots;

        private ItemSlot<ShieldEmitterItemInstance> _shieldEmitter;

        private ItemSlot<CombatUtilityItemInstance>[] _quickSlotItems;

        private Dictionary<WeaponKey, ItemSlot<WeaponItemInstance>> _weapons;

        private int _availableQuickSlots;

        private EventHandler<ItemEventArgs> _itemEquipped;

        private EventHandler<ItemEventArgs> _itemUnequipped;
    }
}
