using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("ARJ")]
    [Serializable]
    public sealed class ARJ : Job
    {
        private ARJ(SerializationInfo info, StreamingContext ctx):base(info, ctx)
        {
        }
    }
}
