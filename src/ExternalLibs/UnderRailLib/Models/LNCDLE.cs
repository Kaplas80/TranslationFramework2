using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("LNCDLE")]
    [Serializable]
    public abstract class LNCDLE : DLE
    {
        protected LNCDLE(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
