using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("OD")]
    [Serializable]
    public sealed class Oddity : NonEquippableItem
    {
        private Oddity(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MinorVersion >= 194)
            {
                _x = info.GetInt32("OD:X");
                _u = info.GetInt32("OD:U");
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("OD:X", _x);
            info.AddValue("OD:U", _u);
        }

        private int _x = 1;

        private int _u = 1;
    }
}
