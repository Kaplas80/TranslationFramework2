using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("OAIM")]
    [Serializable]
    public abstract class OAIM : AIM
    {
        protected OAIM(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
