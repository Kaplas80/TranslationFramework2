using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("SICBDE")]
    [Serializable]
    public sealed class SICBDE : DLE
    {
        private SICBDE(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
