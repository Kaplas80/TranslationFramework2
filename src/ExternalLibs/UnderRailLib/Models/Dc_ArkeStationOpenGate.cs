using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Interfaces;

namespace UnderRailLib.Models
{
    [EncodedTypeName("Dc_ArkeStationOpenGate")]
    [Serializable]
    public sealed class Dc_ArkeStationOpenGate : iLCPP, ISerializable
    {
        private Dc_ArkeStationOpenGate(SerializationInfo info, StreamingContext ctx)
        {
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
        }
    }
}
