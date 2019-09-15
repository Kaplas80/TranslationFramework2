using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Ouroboros.Playfield.Space;

namespace UnderRailLib.Models
{
    [EncodedTypeName("SLAC")]
    [Serializable]
    public sealed class SoftLightAreaContainer : AreaEffectsContainer<SoftLightArea>
    {
        private SoftLightAreaContainer(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
