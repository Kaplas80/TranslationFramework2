using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Interfaces;

namespace UnderRailLib.Models
{
    [EncodedTypeName("MPRFE")]
    [Serializable]
    public sealed class MPRFE : ModifyStatFlatEffect, iMS, iBAFX
    {
        private MPRFE(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
