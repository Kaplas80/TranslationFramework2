using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Interfaces;

namespace UnderRailLib.Models
{
    [EncodedTypeName("Dc_OpenArkePlantToOfficeGate")]
    [Serializable]
    public sealed class Dc_OpenArkePlantToOfficeGate : iLCPP, ISerializable
    {
        private Dc_OpenArkePlantToOfficeGate(SerializationInfo info, StreamingContext ctx)
        {
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
        }
    }
}
