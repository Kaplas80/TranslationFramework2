using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("FOSSATLC")]
    [Serializable]
    public sealed class FOSSATLC : Condition
    {
        private FOSSATLC(SerializationInfo info, StreamingContext ctx):base(info, ctx)
        {
        }
    }
}
