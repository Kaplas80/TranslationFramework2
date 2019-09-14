using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("BCAMDE")]
    [Serializable]
    public sealed class BCAMDE : LNCDLE
    {
        private BCAMDE(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _ca = info.GetBoolean("BCAMDE:CA");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("BCAMDE:CA", _ca);
        }

        private bool _ca;
    }
}
