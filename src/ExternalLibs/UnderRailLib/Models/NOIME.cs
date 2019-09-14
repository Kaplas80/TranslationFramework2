using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Interfaces;

namespace UnderRailLib.Models
{
    [EncodedTypeName("NOIME")]
    [Serializable]
    public sealed class NOIME : MFSE, iMS
    {
        private NOIME(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _S = info.GetInt32("NOIME:S");
            _d = info.GetInt32("NOIME:D");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("NOIME:S", _S);
            info.AddValue("NOIME:D", _d);
        }

        private int _S;

        private int _d = 2;
    }
}
