using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("SMTPA")]
    [Serializable]
    public sealed class SMTPA : BaseAction
    {
        private SMTPA(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _m = (info.GetValue("SMTPA:M", typeof(D2)) as D2);
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("SMTPA:M", _m);
        }

        private D2 _m;
    }
}
