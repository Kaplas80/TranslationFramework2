using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("MRPE")]
    [Serializable]
    public sealed class ModifyResolvePercentageEffect : ModifyStatPercentageEffect
    {
        private ModifyResolvePercentageEffect(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
