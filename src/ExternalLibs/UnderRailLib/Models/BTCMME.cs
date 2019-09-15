using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Interfaces;

namespace UnderRailLib.Models
{
    [EncodedTypeName("BTCMME")]
    [Serializable]
    public sealed class BTCMME : MFSE, iMS
    {
        private BTCMME(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _m = info.GetDouble("BTCMME:M");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("BTCMME:M", _m);
        }

        private double _m;
    }
}
