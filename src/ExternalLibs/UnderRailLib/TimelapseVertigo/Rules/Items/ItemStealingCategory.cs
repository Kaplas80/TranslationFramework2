using System;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.TimelapseVertigo.Rules.Items
{
    [EncodedTypeName("eSC")]
    [Serializable]
    public enum ItemStealingCategory
    {
        Default,
        Money,
        Ammo,
        AmmoHeavy,
        Weapon,
        WeaponHeavy,
        Armor,
        ArmorHeavy,
        ThrownWeapon,
        Trap,
        VehiclePart
    }
}
