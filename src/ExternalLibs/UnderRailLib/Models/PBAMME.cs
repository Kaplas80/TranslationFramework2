using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Interfaces;

namespace UnderRailLib.Models
{
    [EncodedTypeName("PBAMME")]
    [Serializable]
    public sealed class PBAMME : MFSE, iMS
    {
        private PBAMME(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _m = info.GetDouble("PBAMME:M");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("PBAMME:M", _m);
        }

        private double _m;
    }
}
