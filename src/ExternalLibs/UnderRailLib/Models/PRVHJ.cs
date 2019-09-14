using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("PRVHJ")]
    [Serializable]
    public sealed class PRVHJ : Job
    {
        private PRVHJ(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _m = info.GetBoolean("PRVHJ:M");
            _f = info.GetString("PRVHJ:F");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("PRVHJ:M", _m);
            info.AddValue("PRVHJ:F", _f);
        }

        private bool _m;

        private string _f;
    }
}