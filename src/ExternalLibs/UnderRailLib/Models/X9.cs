using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("X9")]
    [Serializable]
    public sealed class X9 : Job
    {
        private X9(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _m = (info.GetValue("X9:M", typeof(TimeSpan?)) as TimeSpan?);
            _p = info.GetString("X9:P");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("X9:M", _m);
            info.AddValue("X9:P", _p);
        }

        private TimeSpan? _m;

        private string _p;
    }
}