using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Interfaces;

namespace UnderRailLib.Models
{
    [EncodedTypeName("XPBLMUSMSIOZ")]
    [Serializable]
    public sealed class XPBLMUSMSIOZ : iLCPP, ISerializable
    {
        private XPBLMUSMSIOZ(SerializationInfo info, StreamingContext ctx)
        {
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
        }
    }
}
