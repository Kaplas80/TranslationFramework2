using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("A50")]
    [Serializable]
    public abstract class A50 : ItemGeneratorBase
    {
        protected A50(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
