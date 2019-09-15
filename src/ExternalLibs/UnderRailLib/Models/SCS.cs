using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("SCS")]
    [Serializable]
    public class SCS : CS
    {
        protected SCS(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
