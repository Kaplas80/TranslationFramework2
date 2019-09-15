using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("MFSE")]
    [Serializable]
    public abstract class MFSE : SE2
    {
        protected MFSE(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
