using System;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.TimelapseVertigo.Rules.Characters
{
    [EncodedTypeName("eSKE")]
    [Serializable]
    public enum SkillEnum
    {
        Guns,
        Throwing,
        Crossbows,
        Melee,
        Dodge = 10,
        Evasion,
        Stealth = 20,
        Hacking,
        Lockpicking,
        Pickpocketing,
        Traps,
        Mechanics = 30,
        Electronics,
        Chemistry,
        Biology,
        Tailoring,
        ThoughtControl = 40,
        Psychokinesis,
        Metathermics,
        TemporalManipulation,
        Persuasion = 50,
        Intimidation,
        Mercantile
    }
}
