using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Interfaces;

namespace UnderRailLib.Models
{
    [EncodedTypeName("XPBLWUMB2AWD")]
    [Serializable]
    public sealed class XPBLWUMB2AWD : iLCPP, ISerializable
    {
        private XPBLWUMB2AWD(SerializationInfo info, StreamingContext ctx)
        {
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
        }
    }
}
