using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("DRAIM")]
    [Serializable]
    public abstract class DRAIM : AIM
    {
        protected DRAIM(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
