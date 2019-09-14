using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("PSRCDE")]
    [Serializable]
    public sealed class PSRCDE : LNCDLE
    {
        private PSRCDE(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _ca = info.GetBoolean("PSRCDE:CA");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("PSRCDE:CA", _ca);
        }

        private bool _ca;
    }
}
