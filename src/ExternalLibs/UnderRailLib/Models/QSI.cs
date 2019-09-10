using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("QSI")]
    [Serializable]
    public class QSI : Item
    {
        protected QSI(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
