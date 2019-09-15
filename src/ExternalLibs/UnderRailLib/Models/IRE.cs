using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Interfaces;

namespace UnderRailLib.Models
{
    [EncodedTypeName("IRE")]
    [Serializable]
    public abstract class IRE : RaysEmitter, iIC, ISerializable
    {
        protected IRE(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
