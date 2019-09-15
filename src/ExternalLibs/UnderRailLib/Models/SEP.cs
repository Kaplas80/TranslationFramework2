using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Enums;

namespace UnderRailLib.Models
{
    [EncodedTypeName("SEP")]
    [Serializable]
    public sealed class SEP : ISerializable
    {
        private SEP(SerializationInfo info, StreamingContext ctx)
        {
            _st = (SST)info.GetValue("SEP:ST", typeof(SST));
            _s = (info.GetValue("SEP:S", typeof(List<string>)) as List<string>);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("SEP:ST", _st);
            info.AddValue("SEP:S", _s);
        }

        private List<string> _s = new List<string>();

        private SST _st;
    }
}
