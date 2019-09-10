using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Interfaces;

namespace UnderRailLib.Models
{
    [EncodedTypeName("SGSDTS")]
    [Serializable]
    public sealed class SGSDTS : iLCPP, ISerializable
    {
        private SGSDTS(SerializationInfo info, StreamingContext ctx)
        {
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
        }
    }
}
