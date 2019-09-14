using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("PVHSJ")]
    [Serializable]
    public sealed class PVHSJ : Job
    {
        private PVHSJ(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
