using System;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.TimelapseVertigo.Rules.Combat
{
    [EncodedTypeName("eWST")]
    [Serializable]
    public enum WeaponSubtype
    {
        Unknown,
        Pistol_5,
        Pistol_762,
        Pistol_9,
        Pistol_44,
        Pistol_Laser,
        Pistol_Plasma,
        Crossbow,
        Sniper_762,
        Sniper_86,
        Sniper_127,
        Smg_5,
        Smg_762,
        Smg_86,
        AssaultRifle_762,
        AssaultRifle_86,
        AssaultRifle_9,
        Pistol_Acid_Blob,
        Pistol_Cryo_Blob,
        Pistol_Indendiary_Blob,
        Pistol_Electro = 30,
        Pistol_Sonic,
        Shotgun_Regular = 51,
        Shotgun_Combat,
        Crowbar = 101,
        Sledgehammer,
        Knife = 200,
        Dagger,
        SerratedKnife,
        Glove_Leather = 301,
        Glove_Metal,
        Spear = 401,
        Sword = 500,
        Machete_Straight,
        Machete_Curved,
        Unarmed = 1000
    }
}
