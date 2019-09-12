using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("SLSJ")]
    [Serializable]
    public sealed class SLSJ : Job
    {
        private SLSJ(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _swfp = info.GetBoolean("PLSJ:SWFP");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("PLSJ:SWFP", _swfp);
        }

        private bool _swfp;
    }
}
