using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Interfaces;

namespace UnderRailLib.Models
{
    [EncodedTypeName("XPBLUPDHWI")]
    [Serializable]
    public sealed class XPBLUPDHWI : iLCPP, ISerializable
    {
        private XPBLUPDHWI(SerializationInfo info, StreamingContext ctx)
        {
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
        }
    }
}
