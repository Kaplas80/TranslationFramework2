using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("FOCSSFJ")]
    [Serializable]
    public sealed class FOCSSFJ : Job
    {
        private FOCSSFJ(SerializationInfo info, StreamingContext ctx):base(info, ctx)
        {
        }
    }
}
