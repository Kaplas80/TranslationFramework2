using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Enums;

namespace UnderRailLib.Models
{
    [EncodedTypeName("DEJ")]
    [Serializable]
    public sealed class DEJ : Job
    {
        private DEJ(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _t = (eSPCJBT)info.GetValue("DEJ:T", typeof(eSPCJBT));
            if (DataModelVersion.MinorVersion >= 32)
            {
                _ct = (info.GetValue("DEJ:CT", typeof(Guid?)) as Guid?);
            }
            if (DataModelVersion.MinorVersion >= 208)
            {
                _d = info.GetBoolean("DEJ:D");
                _s = info.GetBoolean("DEJ:S");
            }
            if (DataModelVersion.MinorVersion >= 326)
            {
                _md = info.GetBoolean("DEJ:MD");
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("DEJ:T", _t);
            info.AddValue("DEJ:CT", _ct);
            info.AddValue("DEJ:D", _d);
            info.AddValue("DEJ:S", _s);
            info.AddValue("DEJ:MD", _md);
        }

        private Guid? _ct;

        private eSPCJBT _t = eSPCJBT.c;
        
        private bool _d = true;

        private bool _s;

        private bool _md;
    }
}
