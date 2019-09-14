using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("JSI")]
    [Serializable]
    public sealed class JetskiSuspensionItem : VSI, ISerializable
    {
        private JetskiSuspensionItem(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
