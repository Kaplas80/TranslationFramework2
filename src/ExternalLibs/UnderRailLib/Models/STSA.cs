using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("STSA")]
    [Serializable]
    public sealed class STSA : BaseAction
    {
        private STSA(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _gp = info.GetString("STSA:GP");
            _ts = (info.GetValue("STSA:TS", typeof(TimeSpan?)) as TimeSpan?);
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("STSA:GP", _gp);
            info.AddValue("STSA:TS", _ts);
        }

        private string _gp;

        private TimeSpan? _ts;
    }
}