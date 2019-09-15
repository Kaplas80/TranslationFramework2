using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("CSN")]
    [Serializable]
    public sealed class CSN : ISerializable
    {
        private CSN(SerializationInfo info, StreamingContext ctx)
        {
            _ds = (info.GetValue("CSN:DS", typeof(SEP)) as SEP);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("CSN:DS", _ds);
        }


        private SEP _ds;
    }
}
