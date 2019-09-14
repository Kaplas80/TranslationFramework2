using System;
using System.ComponentModel;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.TimelapseVertigo.Rules.Characters
{
    [EncodedTypeName("eSKE")]
    [Serializable]
    public enum SkillEnum
    {
        [Description("Guns")]
        Guns,
        [Description("Throwing")]
        Throwing,
        [Description("Crossbows")]
        Crossbows,
        [Description("Melee")]
        Melee,
        [Description("Dodge")]
        Dodge = 10,
        [Description("Evasion")]
        Evasion,
        [Description("Stealth")]
        Stealth = 20,
        [Description("Hacking")]
        Hacking,
        [Description("Lockpicking")]
        Lockpicking,
        [Description("Pickpocketing")]
        Pickpocketing,
        [Description("Traps")]
        Traps,
        [Description("Mechanics")]
        Mechanics = 30,
        [Description("Electronics")]
        Electronics,
        [Description("Chemistry")]
        Chemistry,
        [Description("Biology")]
        Biology,
        [Description("Tailoring")]
        Tailoring,
        [Description("Thought Control")]
        ThoughtControl = 40,
        [Description("Psychokinesis")]
        Psychokinesis,
        [Description("Metathermics")]
        Metathermics,
        [Description("Temporal Manipulation")]
        TemporalManipulation,
        [Description("Persuasion")]
        Persuasion = 50,
        [Description("Intimidation")]
        Intimidation,
        [Description("Mercantile")]
        Mercantile
    }
}
