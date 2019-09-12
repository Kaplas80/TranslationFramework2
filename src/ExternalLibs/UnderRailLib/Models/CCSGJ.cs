using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("CCSGJ")]
    [Serializable]
    public sealed class CCSGJ : Job
    {
        private CCSGJ(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
