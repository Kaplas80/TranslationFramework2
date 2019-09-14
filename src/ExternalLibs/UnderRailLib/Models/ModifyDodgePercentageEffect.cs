using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("MDPE")]
    [Serializable]
    public sealed class ModifyDodgePercentageEffect : ModifyStatPercentageEffect
    {
        private ModifyDodgePercentageEffect(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
