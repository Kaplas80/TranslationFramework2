using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Interfaces;

namespace UnderRailLib.Models
{
    [EncodedTypeName("MCDPE")]
    [Serializable]
    public sealed class MCDPE : MSPE2, iMS
    {
        private MCDPE(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _c = info.GetString("MCDPE:C");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("MCDPE:C", _c);
        }

        private string _c;
    }
}
