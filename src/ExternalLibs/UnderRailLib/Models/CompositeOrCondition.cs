using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("COC")]
    [Serializable]
    public class CompositeOrCondition : CompositeCondition
    {
        protected CompositeOrCondition(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MajorVersion != 0)
            {
                return;
            }
        }
    }
}
