using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Interfaces;

namespace UnderRailLib.Models
{
    [EncodedTypeName("MDFE")]
    [Serializable]
    public sealed class MDFE : ModifyStatFlatEffect, iMS, iDSE
    {
        private MDFE(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
