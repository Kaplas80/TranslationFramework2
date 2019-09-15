using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Interfaces;

namespace UnderRailLib.Models
{
    [EncodedTypeName("MEFE")]
    [Serializable]
    public sealed class ModifyEvasionFlatEffect : ModifyStatFlatEffect, iMS, iDSE
    {
        private ModifyEvasionFlatEffect(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
