using System;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Enums;

namespace UnderRailLib.Models
{
    [EncodedTypeName("SYJ")]
    [Serializable]
    public sealed class bvz : Job
    {
        private bvz(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _t = info.GetString("SYJ:T");
            _s = (eSPCJBT)info.GetValue("SYJ:S", typeof(eSPCJBT));
            _i = info.GetBoolean("SYJ:I");
            _c = (info.GetValue("SYJ:C", typeof(Color?)) as Color?);
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("SYJ:T", _t);
            info.AddValue("SYJ:S", _s);
            info.AddValue("SYJ:I", _i);
            info.AddValue("SYJ:C", _c);
        }

        private string _t;

        private eSPCJBT _s = eSPCJBT.c;

        private bool _i;

        private Color? _c;
    }
}
