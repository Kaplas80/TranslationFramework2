using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("CVWRCFCJ")]
    [Serializable]
    public sealed class CVWRCFCJ : Job
    {
        private CVWRCFCJ(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
