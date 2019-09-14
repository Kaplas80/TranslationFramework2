using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Interfaces;

namespace UnderRailLib.Models
{
    [EncodedTypeName("NOCAME")]
    [Serializable]
    public sealed class NOCAME : MFSE, iMS
    {
        private NOCAME(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _d = info.GetInt32("NOCAME:D");
            _v = info.GetDouble("NOCAME:V");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("NOCAME:D", _d);
            info.AddValue("NOCAME:V", _v);
        }

        private int _d = 3;

        private double _v = 0.25;
    }
}
