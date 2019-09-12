using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("SSSJ")]
    [Serializable]
    public sealed class SSSJ : Job
    {
        private SSSJ(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _d = info.GetInt32("SSSJ:D");
            _lx = (info.GetValue("SSSJ:LX", typeof(int?)) as int?);
            _ux = (info.GetValue("SSSJ:UX", typeof(int?)) as int?);
            _ly = (info.GetValue("SSSJ:LY", typeof(int?)) as int?);
            _uy = (info.GetValue("SSSJ:UY", typeof(int?)) as int?);
            _s = (info.GetValue("SSSJ:S", typeof(int?)) as int?);
        }

        
        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("SSSJ:D", _d);
            info.AddValue("SSSJ:LX", _lx);
            info.AddValue("SSSJ:UX", _ux);
            info.AddValue("SSSJ:LY", _ly);
            info.AddValue("SSSJ:UY", _uy);
            info.AddValue("SSSJ:S", _s);
        }

        private int _d;

        private int? _lx;

        private int? _ux;

        private int? _ly;

        private int? _uy;

        private int? _s;
    }
}
