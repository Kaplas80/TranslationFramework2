using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Interfaces;

namespace UnderRailLib.Models
{
    [EncodedTypeName("M52")]
    [Serializable]
    public sealed class M52 : StatusEffect, iMS
    {
        private M52(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _a = info.GetDouble("M52:A");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("M52:A", _a);
        }

        private double _a;
    }
}
