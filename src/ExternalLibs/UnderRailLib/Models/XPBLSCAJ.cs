using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("XPBLSCAJ")]
    [Serializable]
    public sealed class XPBLSCAJ : Job
    {
        private XPBLSCAJ(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _it = info.GetString("XPBLSCAJ:IT");
            _bn = (info.GetValue("XPBLSCAJ:BN", typeof(int?)) as int?);
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("XPBLSCAJ:IT", _it);
            info.AddValue("XPBLSCAJ:BN", _bn);
        }

        private string _it;

        private int? _bn;
    }
}
