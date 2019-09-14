using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("CIYCPTJ")]
    [Serializable]
    public sealed class CIYCPTJ : Job
    {
        private CIYCPTJ(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _r = info.GetString("CIYCPTJ:R");
            _t = (Point) info.GetValue("CIYCPTJ:T", typeof(Point));
            _a = info.GetString("CIYCPTJ:A");
            _d = info.GetBoolean("CIYCPTJ:D");
            _p = info.GetBoolean("CIYCPTJ:P");
            SerializationHelper.ReadList("CIYCPTJ:M", ref _m, info);
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("CIYCPTJ:R", _r);
            info.AddValue("CIYCPTJ:T", _t);
            info.AddValue("CIYCPTJ:A", _a);
            info.AddValue("CIYCPTJ:D", _d);
            info.AddValue("CIYCPTJ:P", _p);
            SerializationHelper.WriteList("CIYCPTJ:M", _m, info);
        }

        private string _r = "";

        private Point _t;

        private string _a;

        private bool _d;

        private bool _p;

        private List<Guid> _m = new List<Guid>();
    }
}
