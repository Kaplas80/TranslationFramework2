using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Interfaces;

namespace UnderRailLib.Models
{
    [EncodedTypeName("FOSEA")]
    [Serializable]
    public class FOSEA : SEA, iINA, iC1
    {
        protected FOSEA(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
