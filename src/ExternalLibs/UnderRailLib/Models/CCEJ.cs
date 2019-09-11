using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Enums;

namespace UnderRailLib.Models
{
    [EncodedTypeName("CCEJ")]
    [Serializable]
    public sealed class CCEJ : Job
    {
        private CCEJ(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _t = (eSPCJBT) info.GetValue("CCEJ:T", typeof(eSPCJBT));
            _ct = (info.GetValue("CCEJ:CT", typeof(Guid?)) as Guid?);
            if (DataModelVersion.MinorVersion >= 336)
            {
                _b = info.GetBoolean("CCEJ:B");
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("CCEJ:T", _t);
            info.AddValue("CCEJ:CT", _ct);
            info.AddValue("CCEJ:B", _b);
        }

        private Guid? _ct;

        private eSPCJBT _t = eSPCJBT.c;

        private bool _b;
    }
}
