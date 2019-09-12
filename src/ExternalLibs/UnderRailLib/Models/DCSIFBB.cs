using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("DCSIFBB")]
    [Serializable]
    public sealed class DCSIFBB : Job
    {
        private DCSIFBB(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
