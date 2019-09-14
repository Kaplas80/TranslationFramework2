using System;
using System.ComponentModel;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.TimelapseVertigo.Rules.Characters
{
    [EncodedTypeName("eBAE")]
    [Serializable]
    public enum BaseAbilityEnum
    {
        [Description("Strength")]
        Strength,
        [Description("Dexterity")]
        Dexterity,
        [Description("Agility")]
        Agility,
        [Description("Constitution")]
        Constitution,
        [Description("Perception")]
        Perception,
        [Description("Will")]
        Will,
        [Description("Intelligence")]
        Intelligence
    }
}
