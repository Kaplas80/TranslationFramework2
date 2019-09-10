using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("CAC")]
    [Serializable]
    public class CompositeAndCondition : CompositeCondition
    {
        protected CompositeAndCondition(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MajorVersion != 0)
            {
            }
        }
    }
}
