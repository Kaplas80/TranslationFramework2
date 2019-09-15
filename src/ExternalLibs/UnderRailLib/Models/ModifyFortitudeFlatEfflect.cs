using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Interfaces;

namespace UnderRailLib.Models
{
    [EncodedTypeName("MFFE")]
    [Serializable]
    public sealed class ModifyFortitudeFlatEfflect : ModifyStatFlatEffect, iMS
    {
        private ModifyFortitudeFlatEfflect(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
