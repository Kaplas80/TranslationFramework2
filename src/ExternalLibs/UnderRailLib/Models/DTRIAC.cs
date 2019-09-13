using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("DTRIAC")]
    [Serializable]
    public sealed class DTRIAC : Condition
    {
        private DTRIAC(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _r = info.GetString("DTRIAC:R");
            _i = info.GetBoolean("DTRIAC:I");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("DTRIAC:R", _r);
            info.AddValue("DTRIAC:I", _i);
        }

        private string _r;
        private bool _i;
    }
}