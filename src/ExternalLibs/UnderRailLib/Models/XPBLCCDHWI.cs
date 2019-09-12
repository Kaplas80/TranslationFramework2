using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Interfaces;

namespace UnderRailLib.Models
{
    [EncodedTypeName("XPBLCCDHWI")]
    [Serializable]
    public sealed class XPBLCCDHWI : iLCPP, ISerializable
    {
        private XPBLCCDHWI(SerializationInfo info, StreamingContext ctx)
        {
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
        }
    }
}
