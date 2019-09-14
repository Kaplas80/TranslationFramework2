using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Interfaces;

namespace UnderRailLib.Models
{
    [EncodedTypeName("DCCI")]
    [Serializable]
    public sealed class DCCI : MFSE, iMS
    {
        private DCCI(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _m = info.GetDouble("DCCI:M");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("DCCI:M", _m);
        }

        private double _m;
    }
}
