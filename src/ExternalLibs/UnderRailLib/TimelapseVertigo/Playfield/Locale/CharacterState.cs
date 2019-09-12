using System;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.TimelapseVertigo.Playfield.Locale
{
    [EncodedTypeName("eCS")]
    [Serializable]
    public enum CharacterState
    {
        Idle,
        Move,
        Attack1,
        Operating,
        Die,
        CastingPsi,
        FinishPsi,
        ChannelPsi,
        Throw,
        OperateShort,
        Attack2,
        Stunned,
        RaiseWeapon,
        LowerWeapon,
        ShieldBash,
        Custom1 = 101,
        Custom2,
        Custom3,
        Custom4,
        Custom5,
        Custom6
    }
}
