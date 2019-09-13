using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("RNDGEBBRSE")]
    [Serializable]
    public sealed class RNDGEBBRSE : Job
    {
        private RNDGEBBRSE(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
