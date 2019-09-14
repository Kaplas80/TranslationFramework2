using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("PIOBSCC")]
    [Serializable]
    public sealed class PIOBSCC : Condition
    {
        private PIOBSCC(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MinorVersion >= 454)
            {
                _i = info.GetBoolean("PIOBSCC:I");
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("PIOBSCC:I", _i);
        }

        private bool _i;
    }
}
