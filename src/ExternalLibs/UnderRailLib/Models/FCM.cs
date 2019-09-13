using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("FCM")]
    [Serializable]
    public sealed class FCM : Job
    {
        private FCM(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
        }
    }
}
