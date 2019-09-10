using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("R05")]
    [Serializable]
    public sealed class R05 : BaseAction
    {
        private R05(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _m = info.GetInt32("R05:M");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("R05:M", _m);
        }
        
        private int _m;
    }
}
