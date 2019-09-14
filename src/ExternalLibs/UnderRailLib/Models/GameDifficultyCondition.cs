using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.TimelapseVertigo.Rules;

namespace UnderRailLib.Models
{
    [EncodedTypeName("GDC")]
    [Serializable]
    public sealed class GameDifficultyCondition : Condition
    {
        private GameDifficultyCondition(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _mid = (info.GetValue("GDC:MID", typeof(GameDifficulty?)) as GameDifficulty?);
            _mad = (info.GetValue("GDC:MAD", typeof(GameDifficulty?)) as GameDifficulty?);
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("GDC:MID", _mid);
            info.AddValue("GDC:MAD", _mad);
        }

        private GameDifficulty? _mid;

        private GameDifficulty? _mad;
    }
}