using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("G09")]
    [Serializable]
    public sealed class G09 : Job, ISerializable
    {
        private G09(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _qn = info.GetString("G09:QN");
            _pe = info.GetBoolean("G09:PE");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("G09:QN", _qn);
            info.AddValue("G09:PE", _pe);
        }
        
        private string _qn;

        private bool _pe;
    }
}
