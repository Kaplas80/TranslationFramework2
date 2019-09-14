using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Interfaces;

namespace UnderRailLib.Models
{
    [EncodedTypeName("MPF")]
    [Serializable]
    public sealed class MPF : ModifyStatFlatEffect, iMS, iSKFX
    {
        private MPF(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
