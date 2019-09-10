using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.TimelapseVertigo.Rules.Items;

namespace UnderRailLib.Models
{
    [EncodedTypeName("QSII")]
    [Serializable]
    public class QSII : II, IItemInstance<QSI>, ISerializable
    {
        protected QSII(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
