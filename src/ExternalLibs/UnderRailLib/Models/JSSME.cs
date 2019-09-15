using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Interfaces;

namespace UnderRailLib.Models
{
    [EncodedTypeName("JSSME")]
    [Serializable]
    public sealed class JSSME : MFSE, iMS
    {
        private JSSME(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _m = info.GetDouble("JSSME:M");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("JSSME:M", _m);
        }

        private double _m;
    }
}
