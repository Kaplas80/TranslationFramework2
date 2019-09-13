using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("TCHPTRE")]
    [Serializable]
    public sealed class TCHPTRE : Job
    {
        private TCHPTRE(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
