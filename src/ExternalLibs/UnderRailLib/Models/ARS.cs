using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("ARS")]
    [Serializable]
    public sealed class ARS : ISerializable
    {
        private ARS(SerializationInfo info, StreamingContext ctx)
        {
            SerializationHelper.ReadList("ARS:D", ref _d, info);
            SerializationHelper.ReadList("ARS:U", ref _u, info);
            if (DataModelVersion.MinorVersion >= 268)
            {
                _k = info.GetBoolean("ARS:K");
            }
            if (DataModelVersion.MinorVersion >= 283)
            {
                _c = info.GetBoolean("ARS:C");
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            SerializationHelper.WriteList("ARS:D", _d, info);
            SerializationHelper.WriteList("ARS:U", _u, info);
            info.AddValue("ARS:K", _k);
            info.AddValue("ARS:C", _c);
        }

        public List<Damage> _d = new List<Damage>();

        public List<Damage> _u = new List<Damage>();

        public bool _k;

        public bool _c;
    }
}
