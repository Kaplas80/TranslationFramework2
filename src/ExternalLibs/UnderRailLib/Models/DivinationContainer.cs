using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Ouroboros.Playfield.Space;

namespace UnderRailLib.Models
{
    [EncodedTypeName("DC1")]
    [Serializable]
    public sealed class DivinationContainer : AreaEffectsContainer<Divination>
    {
        private DivinationContainer(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
