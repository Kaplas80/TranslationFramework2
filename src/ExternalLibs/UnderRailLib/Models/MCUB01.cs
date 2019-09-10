using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Interfaces;

namespace UnderRailLib.Models
{
    [EncodedTypeName("MCUB01")]
    [Serializable]
    public sealed class MCUB01 : iLCPP, ISerializable
    {
        private MCUB01(SerializationInfo info, StreamingContext ctx)
        {
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
        }
    }
}
