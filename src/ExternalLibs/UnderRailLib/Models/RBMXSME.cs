using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Interfaces;

namespace UnderRailLib.Models
{
    [EncodedTypeName("RBMXSME")]
    [Serializable]
    public sealed class RBMXSME : MFSE, iMS
    {
        private RBMXSME(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _s = info.GetInt32("RBMXSME:S");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("RBMXSME:S", _s);
        }

        private int _s;
    }
}
