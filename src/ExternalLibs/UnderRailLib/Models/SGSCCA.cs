using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("SGSCCA")]
    [Serializable]
    public sealed class SGSCCA : BaseAction
    {
        private SGSCCA(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
