using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("MVDE")]
    [Serializable]
    public sealed class MVDE : DLE
    {
        private MVDE(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
