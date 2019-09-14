using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("MPMAPSC")]
    [Serializable]
    public sealed class MPMAPSC : Condition
    {
        private MPMAPSC(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _mv = info.GetInt32("MPMAPSC:MV");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("MPMAPSC:MV", _mv);
        }

        private int _mv;
    }
}
