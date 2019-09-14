using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("PKDE")]
    [Serializable]
    public sealed class PKDE : LNCDLE
    {
        private PKDE(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _ca = info.GetBoolean("PKDE:CA");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("PKDE:CA", _ca);
        }

        private bool _ca;
    }
}
