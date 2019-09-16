using System;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.TimelapseVertigo.Common
{
    [EncodedTypeName("eTCUR")]
    [Serializable]
    public enum TimelapseCursor
    {
        Default,
        ItemDrag,
        Use,
        ForbiddenUse,
        Impossible,
        InvokeSpell,
        InvokeHostileSpell,
        Attack,
        SpecialAttack,
        AreaTransition,
        Talk,
        Lock,
        Lockpick,
        Hack,
        Pickpocket,
        Power,
        Reload,
        Wait,
        DisarmTrap,
        GroundAttack,
        MultiUse,
        ForbiddenMultiUse,
        InvokeHostileSpellWithChance,
        Observe,
        Disembark,
        Hotwire,
        ForbiddenDisembark,
        ObserveMultiple
    }
}
