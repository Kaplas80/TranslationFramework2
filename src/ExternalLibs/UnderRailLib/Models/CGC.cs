using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Interfaces;
using UnderRailLib.TimelapseVertigo.Rules.Characters;

namespace UnderRailLib.Models
{
    [EncodedTypeName("CGC")]
    [Serializable]
    public sealed class CGC : iCOND, ISerializable
    {
        private CGC(SerializationInfo info, StreamingContext ctx)
        {
            _gender = (Gender)info.GetValue("CGC:G", typeof(Gender));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("CGC:G", _gender);
        }

        private Gender _gender;
    }
}
