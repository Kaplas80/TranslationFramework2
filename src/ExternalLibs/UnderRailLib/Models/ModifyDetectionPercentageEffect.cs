using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("MDPE2")]
    [Serializable]
    public sealed class ModifyDetectionPercentageEffect : ModifyStatPercentageEffect
    {
        private ModifyDetectionPercentageEffect(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
