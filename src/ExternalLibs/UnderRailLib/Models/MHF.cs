using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Interfaces;

namespace UnderRailLib.Models
{
    [EncodedTypeName("MHF")]
    [Serializable]
    public sealed class MHF : ModifyStatFlatEffect, iMS, iBAFX
    {
        private MHF(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
