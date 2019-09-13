using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Interfaces;

namespace UnderRailLib.Models
{
    [EncodedTypeName("LUHRMJ")]
    [Serializable]
    public sealed class LUHRMJ : iLCPP, ISerializable
    {
        private LUHRMJ(SerializationInfo info, StreamingContext ctx)
        {
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
        }
    }
}
