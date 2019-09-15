using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Enums;
using UnderRailLib.Models.Interfaces;

namespace UnderRailLib.Models
{
    [EncodedTypeName("MPACPCE")]
    [Serializable]
    public sealed class MPACPCE : MFSE, iMS
    {
        private MPACPCE(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _c = (ePAC)info.GetValue("MPACACE:C", typeof(ePAC));
            _m = info.GetDouble("MPACACE:M");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("MPACACE:C", _c);
            info.AddValue("MPACACE:M", _m);
        }

        private ePAC _c;

        private double _m;
    }
}
