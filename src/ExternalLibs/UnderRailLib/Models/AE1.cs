using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("AE1")]
    [Serializable]
    public sealed class AE1 : BaseAction
    {
        private AE1(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _xp = info.GetInt32("AE1:XP");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("AE1:XP", _xp);
        }

        private int _xp;
    }
}
