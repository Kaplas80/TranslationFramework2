using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("T5")]
    [Serializable]
    public sealed class Trap : NonEquippableItem
    {
        private Trap(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MinorVersion >= 124)
            {
                _s = info.GetInt32("T5:S");
                _d = info.GetDouble("T5:D");
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("T5:S", _s);
            info.AddValue("T5:D", _d);
        }

        private int _s = 5;

        private double _d = 1.0;
    }
}
