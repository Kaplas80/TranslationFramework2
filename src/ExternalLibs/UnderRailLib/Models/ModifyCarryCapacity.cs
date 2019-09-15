using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Interfaces;

namespace UnderRailLib.Models
{
    [EncodedTypeName("MCC")]
    [Serializable]
    public sealed class ModifyCarryCapacity : ModifyStatFlatEffect, iMS, iSKFX
    {
        private ModifyCarryCapacity(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
