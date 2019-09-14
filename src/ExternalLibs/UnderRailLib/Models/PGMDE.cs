using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("PGMDE")]
    [Serializable]
    public sealed class PGMDE : LNCDLE
    {
        private PGMDE(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _ca = info.GetBoolean("PGMDE:CA");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("PGMDE:CA", _ca);
        }

        private bool _ca;
    }
}
