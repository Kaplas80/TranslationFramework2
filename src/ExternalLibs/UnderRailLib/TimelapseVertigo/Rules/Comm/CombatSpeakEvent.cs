﻿using System;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.TimelapseVertigo.Rules.Comm
{
    [EncodedTypeName("eCSE")]
	[Serializable]
	public enum CombatSpeakEvent
	{
		Unknown,
		Battlecry_General = 10,
		Battlecry_Melee,
		Battlecry_Fists = 30,
		Battlecry_Hammer = 40,
		Battlecry_Knife = 50,
		Battlecry_Spear = 60,
		Battlecry_Sword = 65,
		Battlecry_SingleFirearm = 70,
		Battlecry_Shotgun,
		Battlecry_Burst = 80,
		Battlecry_EnergyWeapon = 100,
		Battlecry_ChemicalWeapon,
		Battlecry_StandardGrenade = 110,
		Battlecry_Firebomb = 120,
		Battlecry_Emp = 130,
		Battlecry_Crossbow = 140,
		Battlecry_Mind = 160,
		Battlecry_Frost = 170,
		Battlecry_Fire = 180,
		Battlecry_Telekinesis = 190,
		Battlecry_Electricity = 200,
		Battlecry_Flashbang = 210,
		Battlecry_Temporal = 220,
		Hit_ResistedAll = 400,
		Hit_ShieldedAll,
		Hit_BlockedAll = 403,
		Hit_General = 410,
		Hit_CriticallyHit = 420,
		Hit_MeleeBlunt = 425,
		Hit_MeleeSharp,
		Hit_Firearm = 430,
		Hit_BurningSensation = 440,
		Hit_Frost = 450,
		Hit_Frozen = 460,
		Hit_Tranquilized = 470,
		Hit_Flashbang = 480,
		Hit_Mind = 490,
		Hit_Bolt = 520,
		Hit_Kneecap = 540,
		Hit_Caltrops = 560,
		Hit_BearTrap = 570,
		Hit_ToxicGas = 590,
		Hit_LowHealth = 620,
		Affliction_Burning = 800,
		Affliction_Chilled = 810,
		Affliction_Rooted = 820,
		Affliction_Enraged = 850,
		Affliction_Poisoned = 860,
		Affliction_Fear = 870,
		Affliction_BrokenRibs = 890,
		Affliction_MentalSubversion = 900,
		Affliction_Bleeding = 910,
		Taunt_General = 1100,
		Taunt_LowHealth = 1120,
		Taunt_Chilled = 1130,
		Taunt_Burning = 1140,
		Taunt_Rooted = 1150,
		Taunt_Stunned = 1160,
		Taunt_Poisoned = 1190,
		Taunt_Bleeding = 1220,
		Taunt_Missed = 1230,
		Consume_Antidote = 1400,
		Consume_Healing = 1410,
		Consume_Painkiller = 1420,
		Consume_Booster = 1430,
		Consume_Psi = 1440,
		Special_HeardNoise = 1600,
		Special_CallForHelp = 1610,
		Special_RemoteCallForHelp = 1620,
		Dying = 2000,
		Custom1 = 3001,
		Custom2,
		Custom3,
		Custom4,
		Custom5
	}
}