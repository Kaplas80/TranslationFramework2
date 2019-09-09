using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("J")]
    [Serializable]
    public class Jump : ConditionalElement, ISerializable
    {
        protected Jump(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MajorVersion != 0)
            {
                return;
            }
        }
    }
}
