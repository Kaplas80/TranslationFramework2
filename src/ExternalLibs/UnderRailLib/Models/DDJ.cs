using System;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Enums;

namespace UnderRailLib.Models
{
    [EncodedTypeName("DDJ")]
    [Serializable]
    public sealed class DDJ : Job
    {
        private DDJ(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _t = info.GetString("SYJ:T");
            _s = (eSPCJBT)info.GetValue("SYJ:S", typeof(eSPCJBT));
            _i = info.GetBoolean("SYJ:I");
            _c = (info.GetValue("SYJ:C", typeof(Color?)) as Color?);
            _ot = (Guid?)info.GetValue("SYJ:OT", typeof(Guid?));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("SYJ:T", _t);
            info.AddValue("SYJ:S", _s);
            info.AddValue("SYJ:I", _i);
            info.AddValue("SYJ:C", _c);
            info.AddValue("SYJ:OT", _ot);
        }

        private string _t;

        private eSPCJBT _s = eSPCJBT.c;

        private bool _i;

        private Color? _c;

        private Guid? _ot;
    }
}
