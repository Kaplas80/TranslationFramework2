using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("G10")]
    [Serializable]
    public sealed class G10 : Job, ISerializable
    {
        private G10(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _qn = info.GetString("G10:QN");
            _pe = info.GetBoolean("G10:PE");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("G10:QN", _qn);
            info.AddValue("G10:PE", _pe);
        }
        
        private string _qn;

        private bool _pe;
    }
}
