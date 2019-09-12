using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("TCHSRTV")]
    [Serializable]
    public sealed class TCHSRTV : Job
    {
        private TCHSRTV(SerializationInfo info, StreamingContext ctx):base(info, ctx)
        {
        }
    }
}
